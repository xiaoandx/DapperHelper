using System;
using System.Configuration;
using DapperHelpers.Core.Enum;
using DapperHelpers.Core.Utils;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SQLite;

namespace DapperHelpers.Core.Repositories
{
  /// <summary>
  /// 数据库连接工厂
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.1</para>
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
          "Database connection address does not meet requirements, please check connection address!",
          e);
      }

      return connection;
    }

    /// <summary>
    /// 创建数据库连接,数据库类型 <see cref="DBType"/> 和指定数据库 <see cref="DBProvider"/>
    /// </summary>
    /// <param name="dBType"></param>
    /// <param name="providers"></param>
    /// <returns></returns>
    public static IDbConnection CreateConnection(DBType dBType, DBProvider providers)
    {
      IDbConnection connection = null;
      //获取连接字符串
      string connectionString = TryGetConnectionString(providers);

      switch (dBType)
      {
        case DBType.SqlServer:
          connection = new SqlConnection(connectionString);
          break;
        case DBType.OleDb:
          connection = new OleDbConnection(connectionString);
          break;
        case DBType.Odbc:
          connection = new OdbcConnection(connectionString);
          break;
        case DBType.Oracle:
          connection = new OracleConnection(connectionString);
          break;
        case DBType.MySql:
          connection = new MySqlConnection(connectionString);
          break;
        case DBType.Sqlite:
          connection = new SQLiteConnection(connectionString);
          break;
        case DBType.PostgreSql:
          connection = new NpgsqlConnection(connectionString);
          break;
        default:
          connection = new OracleConnection(connectionString);
          return null;
      }

      return connection;
    }

    /// <summary>
    /// 创建数据库连接,指定数据库地址settingConnectionString，和数据库类型 <see cref="DBType"/>
    /// </summary>
    /// <param name="dBType"></param>
    /// <param name="settingConnectionString"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IDbConnection CreateConnection(DBType dBType, string settingConnectionString)
    {
      if (string.IsNullOrEmpty(settingConnectionString))
        throw new System.Exception("Database connection string cannot be empty (or does not exist)!");
      IDbConnection connection = null;

      switch (dBType)
      {
        case DBType.SqlServer:
          connection = new SqlConnection(settingConnectionString);
          break;
        case DBType.OleDb:
          connection = new OleDbConnection(settingConnectionString);
          break;
        case DBType.Odbc:
          connection = new OdbcConnection(settingConnectionString);
          break;
        case DBType.Oracle:
          connection = new OracleConnection(settingConnectionString);
          break;
        case DBType.MySql:
          connection = new MySqlConnection(settingConnectionString);
          break;
        case DBType.Sqlite:
          connection = new SQLiteConnection(settingConnectionString);
          break;
        case DBType.PostgreSql:
          connection = new NpgsqlConnection(settingConnectionString);
          break;
        default:
          connection = new OracleConnection(settingConnectionString);
          return null;
      }

      return connection;
    }

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
