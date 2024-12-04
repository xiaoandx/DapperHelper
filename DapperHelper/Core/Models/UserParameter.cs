using System;
using Newtonsoft.Json;

namespace DapperHelper.Core.Models
{
  /// <summary>
  /// 用户参数
  /// </summary>
  public class UserParameter
  {
    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 参数值
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// 是否是集合
    /// </summary>
    public bool IsEnumerable { get; set; }
    
    /// <summary>
    /// 参数类型
    /// </summary>
    private string paramType;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    public UserParameter(Type type, string name, object value, object defaultValue = null)
    {
      this.ParamType = type.FullName;
      this.Name = name;

      if (value == null)
      {
        this.Value = null;
      }
      else
      {
        // 值是枚举类型
        if (this.IsEnumerable) 
        {
          this.Value = JsonConvert.SerializeObject(value);
          if (this.Value.Length == 2)
          {
            // 当前枚举是空集，为了防止in 语句报错，填充默认值
            if (defaultValue != null)
            {
              this.Value = JsonConvert.SerializeObject(new object[] { defaultValue });
            }
          }
        }
        else
        {
          this.Value = value.ToString();
        }
      }
    }
    
    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="obj"></param>
    public void Load(UserParameter obj)
    {
      this.paramType = obj.paramType;
      this.Name = obj.Name;
      this.Value = obj.Value;
    }
    
    /// <summary>
    /// 参数类型
    /// </summary>
    public string ParamType
    {
      get { return paramType; }
      set
      {
        if (value != "System.String" && Type.GetType(value)?.GetInterface("System.Collections.IEnumerable") != null)
        {
          IsEnumerable = true;
        }
        else
        {
          IsEnumerable = false;
        }
        paramType = value;
      }
    }
  }
}