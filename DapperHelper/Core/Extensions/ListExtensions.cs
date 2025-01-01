using System;
using System.Collections.Generic;
using System.Linq;

namespace DapperHelpers.Core.Extensions
{
  /// <summary>
  /// List集合扩展方法
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.1</para>
  /// </remarks>
  public static class ListExtensions
  {
    /// <summary>
    /// List集合如果Count等于零，则添加一个默认类型的Item，避免包含IN关键字SQL执行异常
    /// </summary>
    /// <typeparam name="T">List类型（String，int）</typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="FormatException"></exception>
    public static List<T> IsEmptyAddDefalutItem<T>(this List<T> data)
    {
      if (data == null)
      {
        throw new NullReferenceException("The operation object cannot be NULL. Please check the collection.");
      }

      try
      {
        Type type = typeof(T);
        if (data.Count == 0 || !data.Any())
        {
          if (type == typeof(string))
          {
            data.Add((T)Convert.ChangeType(string.Empty, typeof(T)));
            return data;
          }

          if (type == typeof(int))
          {
            data.Add((T)Convert.ChangeType(default(T), typeof(T)));
            return data;
          }

          if (type.IsClass)
          {
            throw new Exception("SQL IN parameter collections do not support object collections");
          }
        }

        return data;
      }
      catch (Exception e)
      {
        throw new FormatException("Parameter collection type does not match. Please check collection type.", e);
      }
    }

    /// <summary>
    /// Dapper param 预处理，修改List Count等于零的情况
    /// </summary>
    /// <param name="param"></param>
    public static void ParamIsListEmpty(ref object param)
    {
      if (param != null)
      {
        var properties = param.GetType().GetProperties();
        foreach (var property in properties)
        {
          object item = property.GetValue(param);
          Type type = item.GetType();
          if (type == typeof(List<string>))
          {
            List<string> list = (List<string>)item;
            list.IsEmptyAddDefalutItem();
            continue;
          }

          if (type == typeof(List<int>))
          {
            List<int> list = (List<int>)item;
            list.IsEmptyAddDefalutItem();
            continue;
          }
        }
      }
    }
  }
}