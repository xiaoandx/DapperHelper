using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperHelpers.Core.Models
{
  /// <summary>
  /// UserCommand
  /// </summary>
  public class UserCommand
  {
    /// <summary>
    /// 执行SQL
    /// </summary>
    public string CommandText { get; set; }

    /// <summary>
    /// 参数集合
    /// </summary>
    public List<UserParameter> Parameters { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="commandText"></param>
    public UserCommand(string commandText)
    {
      this.CommandText = commandText;
      this.Parameters = new List<UserParameter>();
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="commandText"></param>
    /// <param name="userObjectParameter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public UserCommand(string commandText, object userObjectParameter)
    {
      if (userObjectParameter == null)
        throw new System.ArgumentNullException(nameof(userObjectParameter) + "object cannot be empty");

      this.CommandText = commandText;
      List<UserParameter> userParameters = new List<UserParameter>();
      Type type = userObjectParameter.GetType();
      foreach (var property in type.GetProperties())
      {
        if (property.PropertyType.FullName != "System.String" &&
            property.PropertyType.GetInterface("System.Collections.IEnumerable") != null)
        {
          throw new Exception(
            $"{property.PropertyType.FullName} for enumeration types, it is necessary to use the SetParam method to set parameters");
        }

        userParameters.Add(new UserParameter(property.PropertyType, property.Name,
          property.GetValue(userObjectParameter)));
      }

      this.Parameters = userParameters;
    }

    /// <summary>
    /// 用於设置SQL参数, 提供方法实现逐个参数进行传参，可以传递列表参数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="Exception"></exception>
    public void SetParameter<T>(string name, T value, object defaultValue = null)
    {
      Type type = typeof(T);

      var data = new UserParameter(type, name, value, defaultValue);

      // 删掉已存在的参数值
      var existItem = this.Parameters.FirstOrDefault(a => a.Name == name);
      if (existItem != null)
      {
        existItem.Load(data);
        data = existItem;
      }
      else
      {
        // 使用当前参数值
        if (data.IsEnumerable == true)
        {
          if (defaultValue == null)
          {
            throw new Exception(
              $"The current SQL parameter :{data.Name} is an enumerated value, and a defaultValue is required to prevent exceptions when the enumeration list is empty");
          }

          if (this.Parameters.Count(a => a.IsEnumerable) > 0)
          {
            throw new Exception($"Only one enumeration list parameter can be passed in");
          }

          // 为枚举参数重命名
          string newName = $"IsEnumerableParameter";
          this.CommandText = this.CommandText.Replace($":{data.Name}", $":{newName}");
          data.Name = newName;
        }

        this.Parameters.Add(data);
      }
    }
  }
}