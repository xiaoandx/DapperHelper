using System;
using System.Configuration;
using DapperHelpers.Core.Enum;
using DapperHelpers.Core.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DapperHelpers.Core.Repositories
{
  /// <summary>
  /// 数据库连接工厂
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.0</para>
  /// </remarks>
  public static class DBFactory
  {
    /// <summary>
    /// 创建数据库连接
    /// </summary>
    /// <returns></returns>
    public static IDbConnection CreateConnection()
    {
      return CreateConnection(DBProvider.MESCon);
    }

    /// <summary>
    /// 创建数据库连接,指定数据库 <see cref="DBProvider"/>
    /// </summary>
    /// <param name="providers"></param>
    /// <returns></returns>
    public static IDbConnection CreateConnection(DBProvider providers)
    {
      IDbConnection connection = null;
      //获取连接字符串
      string connectionString = TryGetConnectionString(providers);

      connection = new OracleConnection(connectionString);

      return connection;
    }

    /// <summary>
    /// 创建数据库连接,指定数据库连接字符串
    /// </summary>
    /// <param name="settingConnectionString"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public static IDbConnection CreateConnection(string settingConnectionString)
    {
      if (string.IsNullOrEmpty(settingConnectionString))
        throw new System.Exception("Database connection string cannot be empty (or does not exist)!");
      IDbConnection connection = null;
      //获取连接字符串
      string connectionString = settingConnectionString;

      try
      {
        connection = new OracleConnection(connectionString);
      }
      catch (System.Exception e)
      {
        throw new System.Exception(
          "Database connection address does not meet requirements, please check connection address!", e);
      }

      return connection;
    }

    //public static IDbConnection CreateConnection(DBProvider providers, DBType dBType)
    //{
    //    IDbConnection connection = null;
    //    //获取连接字符串
    //    string connectionString = TryGetConnectionString(providers);

    //    switch (dBType)
    //    {
    //        case DBType.SqlServer:
    //            connection = new SqlConnection();
    //            break;
    //        case DBType.OleDb:
    //            connection = new OleDbConnection();
    //            break;
    //        case DBType.Odbc:
    //            connection = new OdbcConnection();
    //            break;
    //        case DBType.Oracle:
    //            connection = new OracleConnection();
    //            break;
    //        case DBType.MySql:
    //            connection = new MySqlConnection();
    //            break;
    //        default:
    //            connection = new OracleConnection(connectionString);
    //            return null;
    //    }

    //    return connection;
    //}

    /// <summary>
    /// 获取数据库连接字符串 
    /// </summary>
    /// <param name="providers"></param>
    /// <returns></returns>
    /// <exception cref="ConfigurationException"></exception>
    public static string TryGetConnectionString(DBProvider providers)
    {
      string connString;
      try
      {
        connString = ConfigManager.GetAppSettingString(providers.ToString());
        //ConfigurationManager.ConnectionStrings[providers.ToString()].ConnectionString;
      }
      catch
      {
        try
        {
          connString = ConfigurationManager.ConnectionStrings[providers.ToString()].ConnectionString;
        }
        catch
        {
          throw new Exception("Connection string does not exist!");
        }
      }

      return connString;
    }
  }
}