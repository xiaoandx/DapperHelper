using Dapper;
using Dapper.Contrib.Extensions;
using DapperHelpers.Core.Extensions;
using DapperHelpers.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DapperHelpers.Core.Models;
using Newtonsoft.Json;

namespace DapperHelpers
{
  /// <summary>
  /// Dapper数据操作基类 v1.6.0.0
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.0</para>
  /// </remarks>
  /// <typeparam name="TEntity">仓储实体</typeparam>
  public class DapperHelper<TEntity> : IRepository<TEntity> where TEntity : class, new()
  {
    /// <summary>
    /// 默认连接对象进行创建Connection对象
    /// </summary>
    public DapperHelper()
    {
      this.Connection = DBFactory.CreateConnection();
    }

    /// <summary>
    /// 指定DBProvider进行创建Connection对象
    /// <para>
    /// DBProvider(MESCon,InterfaceDB,MESLogCon,MESOtherCon)
    /// </para>
    /// </summary>
    /// <param name="providers"></param>
    public DapperHelper(DBProvider providers)
    {
      this.Connection = DBFactory.CreateConnection(providers);
    }

    /// <summary>
    /// 指定数据库连接地址进行创建Connection对象
    /// </summary>
    /// <param name="settingConnectionString">配置文件中的数据库连接地址</param>
    public DapperHelper(string settingConnectionString)
    {
      this.Connection = DBFactory.CreateConnection(settingConnectionString);
    }

    /// <summary>
    /// 指定dbConnection进行创建Connection对象
    /// </summary>
    /// <param name="dbConnection"></param>
    public DapperHelper(IDbConnection dbConnection)
    {
      this.Connection = dbConnection;
    }

    /// <summary>
    /// DB Connection
    /// </summary>
    public IDbConnection Connection { get; private set; }

    #region Connection基础操作

    /// <summary>
    /// 打开数据库连接,如果关闭或者连接中断,则打开连接
    /// </summary>
    public void OpenConnection(IDbConnection conn)
    {
      if (conn.State == ConnectionState.Closed)
      {
        conn.Open();
      }
      else if (conn.State == ConnectionState.Broken)
      {
        conn.Close();
        conn.Open();
      }
    }

    /// <summary>
    /// 打开数据库连接,如果关闭或者连接中断,则打开连接
    /// </summary>
    public void OpenConnection()
    {
      if (Connection.State == ConnectionState.Closed)
      {
        Connection.Open();
      }
      else if (Connection.State == ConnectionState.Broken)
      {
        Connection.Close();
        Connection.Open();
      }
    }

    /// <summary>
    /// 以指定的 System.Data.IsolationLevel 值开始一个数据库事务。
    /// </summary>
    /// <param name="il">指定连接的事务锁定行为</param>
    /// <returns>表示新事务的对象</returns>
    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
      OpenConnection(this.Connection);
      return this.Connection.BeginTransaction(il);
    }

    /// <summary>
    /// 开始一个数据库事务
    /// </summary>
    /// <returns></returns>
    public IDbTransaction BeginTransaction()
    {
      OpenConnection(this.Connection);
      return this.Connection.BeginTransaction();
    }

    /// <summary>
    /// 为打开的 Connection 对象更改当前数据库
    /// </summary>
    /// <param name="dataBaseName">要代替当前数据库进行使用的数据库的名称</param>
    public void ChangeDataBase(string dataBaseName)
    {
      this.Connection.ChangeDatabase(dataBaseName);
    }

    /// <summary>
    /// 创建并返回一个与该连接相关联的 Command 对象
    /// </summary>
    /// <returns>一个与该连接相关联的 Command 对象</returns>
    public IDbCommand CreateCommand()
    {
      return this.Connection.CreateCommand();
    }

    #endregion

    /// <summary>
    /// Dapper 执行SQL返回DataTable类型
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    [Obsolete("该函数已弃用，请使用QueryDataTable函数替代")]
    public DataTable Query_DataTable(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        return dt;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Query_DataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    [Obsolete("该函数已弃用，请使用QueryDataTableIn函数替代")]
    public DataTable Query_DataTableIn(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        return dt;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Dapper 执行SQL返回DataTable类型
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public DataTable QueryDataTable(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        return dt;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryDataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public DataTable QueryDataTableIn(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        return dt;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Dapper 执行SQL返回DataTable第First行数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    [Obsolete("该函数已弃用，请使用QueryDataTableFirstRow函数替代")]
    public DataRow Query_DataTableFirstRow(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        if (dt == null || dt.Rows.Count == 0)
          throw new Exception("No data found, please check the data source or SQL！");

        return dt.FirstRow();
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Query_DataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    [Obsolete("该函数已弃用，请使用QueryDataTableInFirstRow函数替代")]
    public DataRow Query_DataTableInFirstRow(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        if (dt == null || dt.Rows.Count == 0)
          throw new Exception("No data found, please check the data source or SQL！");

        return dt.FirstRow();
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Dapper 执行SQL返回DataTable类型
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public DataRow QueryDataTableFirstRow(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        if (dt == null || dt.Rows.Count == 0)
          throw new Exception("No data found, please check the data source or SQL！");
        return dt.FirstRow();
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryDataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public DataRow QueryDataTableInFirstRow(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);
        if (dt == null || dt.Rows.Count == 0)
          throw new Exception("No data found, please check the data source or SQL！");
        return dt.FirstRow();
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryString 执行SQL返回string类型的單個數據
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public string QueryString(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true,
      int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);

        if (dt != null && dt.Rows.Count > 0)
          return Convert.ToString(dt.Rows[0][0]);
        else
          return null;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryStringIn 执行SQL返回String类型的單個數據,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public string QueryStringIn(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        DataTable dt = new DataTable();
        dt.Load(d);

        if (dt != null && dt.Rows.Count > 0)
          return Convert.ToString(dt.Rows[0][0]);
        else
          return null;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 单条SQL，多次执行（每次执行传递数据不同），该方法执行SQL采用事务控制
    /// <para>
    /// 默认调用MesCon连接字符串
    /// </para>
    /// </summary>
    /// <param name="sqlParamDir"></param>
    /// <exception cref="Exception"></exception>
    public void DapperExecuteBatch(Dictionary<string, List<object>> sqlParamDir)
    {
      try
      {
        OpenConnection(this.Connection);

        using (var trans = Connection.BeginTransaction())
        {
          try
          {
            foreach (var sqlParam in sqlParamDir)
            {
              string sql = sqlParam.Key;
              //判断SQL结尾是否有分号【;】，存在分号就将其裁切掉
              if (sql.EndsWith(";"))
              {
                sql = sql.Substring(0, sql.Length - 1);
              }

              if (sqlParam.Value.Count > 0)
              {
                sqlParam.Value.ForEach(param => { Connection.Execute(sql, param, trans); });
              }
              else
              {
                Connection.Execute(sql, trans);
              }
            }

            trans.Commit();
          }
          catch
          {
            trans.Rollback();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 单条SQL，多次执行（每次执行传递数据不同），该方法执行SQL采用事务控制
    /// <para>
    /// 默认调用MesCon连接字符串
    /// </para>
    /// <para>
    /// 该方法执行出现错误，可以通过Out返回错误信息
    /// </para>
    /// </summary>
    /// <param name="sqlParamDir"></param>
    /// <param name="errorMsg"></param>
    /// <exception cref="Exception"></exception>
    public void DapperExecuteBatch(Dictionary<string, List<object>> sqlParamDir, out string errorMsg)
    {
      try
      {
        OpenConnection(this.Connection);

        using (var trans = Connection.BeginTransaction())
        {
          try
          {
            foreach (var sqlParam in sqlParamDir)
            {
              string sql = sqlParam.Key;
              //判断SQL结尾是否有分号【;】，存在分号就将其裁切掉
              if (sql.EndsWith(";"))
              {
                sql = sql.Substring(0, sql.Length - 1);
              }

              if (sqlParam.Value.Count > 0)
              {
                sqlParam.Value.ForEach(param => { Connection.Execute(sql, param, trans); });
              }
              else
              {
                Connection.Execute(sql, trans);
              }
            }

            trans.Commit();

            errorMsg = string.Empty;
          }
          catch (Exception ex)
          {
            trans.Rollback();
            errorMsg = ex.Message + ex.StackTrace;
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合批量（循环）执行
    /// <para>
    /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）；
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public int BatchExecutionForeach(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 0;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            foreach (var item in list)
            {
              string sqlOriginal = item.Trim();
              // 确保SQL不以【;】结尾
              if (sqlOriginal.EndsWith(";") || sqlOriginal.EndsWith("；"))
              {
                sqlOriginal = sqlOriginal.Substring(0, sqlOriginal.Length - 1);
              }

              int ResultRow = this.Connection.Execute(sqlOriginal, transaction: transation);
              if (ResultRow >= 0)
              {
                ExecutionRow += ResultRow;
              }
            }

            transation.Commit();
            return ExecutionRow;
          }
          catch
          {
            transation.Rollback();
            ExecutionRow = -1;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合批量（循环）执行
    /// <para>
    /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）；
    /// </para>
    /// <para>
    /// 该方法执行出现错误，可以通过Out返回错误信息
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <param name="errorMsg">异常内容</param>
    /// <returns>执行状态</returns>
    public int BatchExecutionForeach(List<string> list, out string errorMsg)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 0;
        string execultSQL = string.Empty;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            foreach (var item in list)
            {
              string sqlOriginal = item.Trim();
              // 确保SQL不以【;】结尾
              if (sqlOriginal.EndsWith(";") || sqlOriginal.EndsWith("；"))
              {
                sqlOriginal = sqlOriginal.Substring(0, sqlOriginal.Length - 1);
              }

              execultSQL = sqlOriginal;
              int ResultRow = this.Connection.Execute(sqlOriginal, transaction: transation);
              if (ResultRow >= 0)
              {
                ExecutionRow += ResultRow;
              }
            }

            transation.Commit();

            errorMsg = string.Empty;
            return ExecutionRow;
          }
          catch (Exception ex)
          {
            transation.Rollback();

            errorMsg = ex.Message + $"exception sql[{execultSQL}]";
            ExecutionRow = -1;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
    /// <para>
    /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误，或者SQL结尾未加[;]
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public int BatchExecutionBeginEnd(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 1;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            string sql = $@"begin
";
            foreach (string item in list)
            {
              string tempSql = item;
              if (!tempSql.EndsWith(";"))
              {
                tempSql += ";";
              }

              sql += tempSql + "\r\n";
            }

            sql += "end;";

            int result = Connection.Execute(sql, transaction: transation);
            transation.Commit();
            return ExecutionRow;
          }
          catch
          {
            transation.Rollback();
            ExecutionRow = 0;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
    /// <para>
    /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误，或者SQL结尾未加[;]
    /// </para>
    /// <para>
    /// 该方法执行出现错误，可以通过Out返回错误信息
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <param name="errorMsg">异常错误信息</param>
    /// <returns>执行状态</returns>
    public int BatchExecutionBeginEnd(List<string> list, out string errorMsg)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 1;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            string sql = $@"begin
";
            foreach (string item in list)
            {
              string tempSql = item;
              if (!tempSql.EndsWith(";"))
              {
                tempSql += ";";
              }

              sql += tempSql + "\r\n";
            }

            sql += "end;";

            int result = Connection.Execute(sql, transaction: transation);
            transation.Commit();

            errorMsg = string.Empty;
            return ExecutionRow;
          }
          catch (Exception ex)
          {
            transation.Rollback();

            errorMsg = ex.Message + ex.StackTrace;
            ExecutionRow = 0;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)SQL集合批量（循环）执行
    /// <para>
    /// return [Row] 表示执行成功返回受影响行数；执行出现异常将转抛异常
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行完成受影响的行数</returns>
    public int BatchExecutionForeachNTS(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 0;
        try
        {
          foreach (var item in list)
          {
            string sqlOriginal = item.Trim();
            // 确保SQL不以【;】结尾
            if (sqlOriginal.EndsWith(";") || sqlOriginal.EndsWith("；"))
            {
              sqlOriginal = sqlOriginal.Substring(0, sqlOriginal.Length - 1);
            }

            int ResultRow = this.Connection.Execute(sqlOriginal);
            if (ResultRow >= 0)
            {
              ExecutionRow += ResultRow;
            }
          }

          return ExecutionRow;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message, ex);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
    /// <para>
    /// return [1] 表示执行成功；执行异常将抛出异常信息
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public int BatchExecutionBeginEndNTS(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionRow = 1;
        try
        {
          string sql = $@"begin
";
          foreach (string item in list)
          {
            string tempSql = item;
            if (!tempSql.EndsWith(";"))
            {
              tempSql += ";";
            }

            sql += tempSql + "\r\n";
          }

          sql += "end;";
          int result = Connection.Execute(sql);
          return ExecutionRow;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message, ex);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合批量（循环）执行
    /// <para>
    /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）；
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public async Task<int> BatchExecutionForeachAsync(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);
        int ExecutionRow = 0;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            foreach (var item in list)
            {
              string sqlOriginal = item.Trim();
              // 确保SQL不以【;】结尾
              if (sqlOriginal.EndsWith(";") || sqlOriginal.EndsWith("；"))
              {
                sqlOriginal = sqlOriginal.Substring(0, sqlOriginal.Length - 1);
              }

              int ResultRow = await Connection.ExecuteAsync(sqlOriginal, transaction: transation);
              ExecutionRow += ResultRow;
            }

            transation.Commit();
            return ExecutionRow;
          }
          catch
          {
            transation.Rollback();
            ExecutionRow = -1;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
    /// <para>
    /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误，或者SQL结尾未加[;]
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public async Task<int> BatchExecutionBeginEndAsync(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);
        int ExecutionRow = 1;
        using (var transation = this.Connection.BeginTransaction())
        {
          try
          {
            string sql = $@"begin
";
            foreach (string item in list)
            {
              string tempSql = item;
              if (!tempSql.EndsWith(";"))
              {
                tempSql += ";";
              }

              sql += tempSql + "\r\n";
            }

            sql += "end;";

            int result = await Connection.ExecuteAsync(sql, transaction: transation);
            transation.Commit();
            return ExecutionRow;
          }
          catch
          {
            transation.Rollback();
            ExecutionRow = 0;
          }
        }

        return ExecutionRow;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)SQL集合批量（循环）执行
    /// <para>
    /// return [Row] 表示执行成功返回受影响行数；执行出现异常将转抛异常
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行完成受影响的行数</returns>
    public async Task<int> BatchExecutionForeachAsyncNTS(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);
        int ExecutionRow = 0;
        try
        {
          foreach (var item in list)
          {
            string sqlOriginal = item.Trim();
            // 确保SQL不以【;】结尾
            if (sqlOriginal.EndsWith(";") || sqlOriginal.EndsWith("；"))
            {
              sqlOriginal = sqlOriginal.Substring(0, sqlOriginal.Length - 1);
            }

            int ResultRow = await Connection.ExecuteAsync(sqlOriginal);
            ExecutionRow += ResultRow;
          }

          return ExecutionRow;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message, ex);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
    /// <para>
    /// return [1] 表示执行成功；执行异常将抛出异常信息
    /// </para>
    /// </summary>
    /// <param name="list">SQL集合</param>
    /// <returns>执行状态</returns>
    public async Task<int> BatchExecutionBeginEndAsyncNTS(List<string> list)
    {
      try
      {
        OpenConnection(this.Connection);
        int ExecutionRow = 1;
        try
        {
          string sql = $@"begin
";
          foreach (string item in list)
          {
            string tempSql = item;
            if (!tempSql.EndsWith(";"))
            {
              tempSql += ";";
            }

            sql += tempSql + "\r\n";
          }

          sql += "end;";
          int result = await Connection.ExecuteAsync(sql);
          return ExecutionRow;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message, ex);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <param name="ProcName">存储过程名称</param>
    /// <param name="dynamicParameters">存储过程参数对象（包含input，output）</param>
    /// <returns>执行存储过程状态</returns>
    public int ExecuteProcedure(string ProcName, DynamicParameters dynamicParameters)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionStatus = 1;
        using (var transaction = this.Connection.BeginTransaction())
        {
          try
          {
            int result = Connection.Execute(ProcName, dynamicParameters, commandType: CommandType.StoredProcedure);
            transaction.Commit();
            return ExecutionStatus;
          }
          catch
          {
            transaction.Rollback();
            ExecutionStatus = 0;
          }
        }

        return ExecutionStatus;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 执行存储过程
    /// <para>
    /// 该方法执行出现错误，可以通过Out返回错误信息
    /// </para>
    /// </summary>
    /// <param name="ProcName">存储过程名称</param>
    /// <param name="dynamicParameters">存储过程参数对象（包含input，output）</param>
    /// <param name="errorMsg"></param>
    /// <returns>执行存储过程状态</returns>
    public int ExecuteProcedure(string ProcName, DynamicParameters dynamicParameters, out string errorMsg)
    {
      try
      {
        OpenConnection(this.Connection);

        int ExecutionStatus = 1;
        using (var transaction = this.Connection.BeginTransaction())
        {
          try
          {
            int result = Connection.Execute(ProcName, dynamicParameters, commandType: CommandType.StoredProcedure);
            transaction.Commit();

            errorMsg = string.Empty;
            return ExecutionStatus;
          }
          catch (Exception ex)
          {
            transaction.Rollback();

            errorMsg = ex.Message;
            ExecutionStatus = 0;
          }
        }

        return ExecutionStatus;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// ExecuteSqlListAsync,异步批量执行Sql List
    /// </summary>
    /// <param name="sqlList">SQLS对象集合</param>
    /// <returns></returns>
    public async Task ExecuteSqlListAsync(IEnumerable<string> sqlList)
    {
      try
      {
        OpenConnection(this.Connection);
        using (var transaction = Connection.BeginTransaction())
        {
          try
          {
            foreach (var sql in sqlList)
            {
              await Connection.ExecuteAsync(sql, transaction: transaction);
            }

            transaction.Commit();
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// ExecuteSqlListAsyncResultRows,异步批量执行Sql List
    /// </summary>
    /// <param name="sqlList">SQLS对象集合</param>
    /// <returns>执行结果</returns>
    public async Task<int> ExecuteSqlListAsyncResultRows(IEnumerable<string> sqlList)
    {
      try
      {
        OpenConnection(this.Connection);
        int ResultRows = 0;
        using (var transaction = Connection.BeginTransaction())
        {
          try
          {
            foreach (var sql in sqlList)
            {
              int status = await Connection.ExecuteAsync(sql, transaction: transaction);
              ResultRows += status;
            }

            transaction.Commit();
            return ResultRows;
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)ExecuteSqlListAsync,异步批量执行Sql List，取消事务管理
    /// </summary>
    /// <param name="sqlList">SQLS对象集合</param>
    /// <returns>执行异常会抛出对应错误异常</returns>
    public async Task ExecuteSqlListAsyncNTS(IEnumerable<string> sqlList)
    {
      OpenConnection(this.Connection);
      try
      {
        foreach (var sql in sqlList)
        {
          await Connection.ExecuteAsync(sql);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// (无事务)ExecuteSqlListAsyncResultRowsNTS,异步批量执行Sql List，取消事务管理，执行成功返回受影响的行数
    /// </summary>
    /// <param name="sqlList">SQLS对象集合</param>
    /// <returns>执行成功返回受影响的行数，执行异常会抛出对应错误异常</returns>
    public async Task<int> ExecuteSqlListAsyncResultRowsNTS(IEnumerable<string> sqlList)
    {
      int resultRows = 0;
      OpenConnection(this.Connection);
      try
      {
        foreach (var sql in sqlList)
        {
          int status = await Connection.ExecuteAsync(sql);
          resultRows += status;
        }

        return resultRows;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 根据主键获取实体数据
    /// </summary>
    /// <param name="key">主键值</param>
    /// <returns></returns>
    public virtual TEntity Get(object key)
    {
      try
      {
        OpenConnection(this.Connection);
        return Get<TEntity>(key);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    ///  根据主键获取实体数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="traction"></param>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public TEntity Get(object key, IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Get<TEntity>(key, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 根据主键获取实体数据
    /// </summary>
    /// <param name="key">主键值</param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public async Task<TEntity> GetAsync(object key, IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return await GetAsync<TEntity>(key);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 根据主键获取实体数据
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="key">主键值</param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public T Get<T>(object key, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return Connection.Get<T>(key, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 根据主键获取实体数据
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="key">主键值</param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(object key, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.GetAsync<T>(key, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 获取所有的实体数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TEntity> GetAll()
    {
      try
      {
        OpenConnection(this.Connection);
        return GetAll<TEntity>();
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 获取所有的实体数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TEntity> GetAll(IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.GetAll<TEntity>(traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 获取所有的实体数据
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync(IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.GetAllAsync<TEntity>(traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 获取所有的实体数据
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetAll<T>(IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return Connection.GetAll<T>(traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 获取所有的实体数据
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>(IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.GetAllAsync<T>(traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 插入实体数据
    /// </summary>
    /// <param name="entityToInsert"></param>
    /// <returns></returns>
    public bool Insert(TEntity entityToInsert)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Connection.Insert<TEntity>(entityToInsert) > 0;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 插入实体数据
    /// </summary>
    /// <param name="entityToInsert"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>    
    /// <returns></returns>
    public bool Insert(TEntity entityToInsert, IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Insert<TEntity>(entityToInsert, traction, commandTimeout) > 0;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 插入实体数据
    /// </summary>
    /// <param name="entityToInsert"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>    
    /// <param name="sqlAdapter"></param>
    /// <returns></returns>
    public async Task<bool> InsertAsync(TEntity entityToInsert, IDbTransaction traction = null,
      int? commandTimeout = null, ISqlAdapter sqlAdapter = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return await InsertAsync<TEntity>(entityToInsert, traction, commandTimeout) > 0;
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 插入实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToInsert"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public long Insert<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return Connection.Insert<T>(entityToInsert, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 插入实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToInsert"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="sqlAdapter"></param>
    /// <returns></returns>
    public async Task<long> InsertAsync<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null,
      ISqlAdapter sqlAdapter = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.InsertAsync<T>(entityToInsert, traction, commandTimeout, sqlAdapter);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 更新实体数据
    /// </summary>
    /// <param name="entityToUpdate"></param>
    /// <returns></returns>
    public bool Update(TEntity entityToUpdate)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Connection.Update<TEntity>(entityToUpdate);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    ///  更新实体数据
    /// </summary>
    /// <param name="entityToUpdate"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public bool Update(TEntity entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Update<TEntity>(entityToUpdate, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 更新实体数据
    /// </summary>
    /// <param name="entityToUpdate"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity entityToUpdate, IDbTransaction traction = null,
      int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return await UpdateAsync<TEntity>(entityToUpdate);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 更新实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToUpdate"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public bool Update<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return Connection.Update<T>(entityToUpdate, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 更新实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToUpdate"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.UpdateAsync<T>(entityToUpdate, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 删除实体数据
    /// </summary>
    /// <param name="entityToDelete"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public bool Delete(TEntity entityToDelete, IDbTransaction traction = null, int? commandTimeout = null)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Delete<TEntity>(entityToDelete, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    ///  删除实体数据
    /// </summary>
    /// <param name="entityToDelete"></param>
    /// <returns></returns>
    public bool Delete(TEntity entityToDelete)
    {
      try
      {
        OpenConnection(this.Connection);
        return this.Connection.Delete<TEntity>(entityToDelete);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 删除实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToDelete"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public bool Delete<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return Connection.Delete(entityToDelete, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 删除实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entityToDelete"></param>
    /// <param name="traction">此次操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);
        return await Connection.DeleteAsync<T>(entityToDelete, traction, commandTimeout);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 执行原生sql
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <returns></returns>
    public int Execute(string sql, object param = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.Execute(sql, param);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 执行原生sql
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <returns></returns>
    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.ExecuteAsync(sql, param);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 执行原生sql
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null,
      CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.Execute(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 执行原生sql
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 批量执行 UserCommand
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public int Execute(UserCommands commands, int? commandTimeout = null, CommandType? commandType = null)
    {
      if (commands.Count == 0)
        return 0;

      return Execute(commands.AsEnumerable(), commandTimeout, commandType);
    }

    /// <summary>
    /// 批量执行 UserCommand
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private int Execute(IEnumerable<UserCommand> commands, int? commandTimeout = null, CommandType? commandType = null)
    {
      int executeCount = 0;

      using (var trans = this.BeginTransaction())
      {
        try
        {
          foreach (var command in commands)
          {
            var commandTemp = command;
            // 提取SQL參數
            string commandText = commandTemp.CommandText;
            DynamicParameters tempParameters = null;

            #region Extract SQL parameters to DynamicParameters

            var parameterList = commandTemp.Parameters;
            if (parameterList.Count > 0)
            {
              tempParameters = new DynamicParameters();

              #region Detecting parameter missing transmission

              string tempCommandText = commandText.ToString();
              // 按照调用时传递的参数擦除当前SQL对应的参数栏位：如 Name=:Name 变成 Name=Name
              foreach (var item in parameterList)
              {
                tempCommandText = tempCommandText.Replace($":{item.Name}", item.Name);
              }

              // 检测擦除参数栏位之后，SQL里面是否还残留 “:”
              tempCommandText = tempCommandText.Replace(" ", "").ToUpper(); //去除空格
              if (tempCommandText.Contains("=:") || tempCommandText.Contains("IN:"))
              {
                throw new Exception("Parameterized SQL must upload all required parameters");
              }

              #endregion
            }

            foreach (var item in parameterList)
            {
              UserParameter val = item;
              object paramValue = null;

              // 判断参数是否枚举值
              bool isEnumerable = Type.GetType(val.ParamType)?.GetInterface("System.Collections.IEnumerable") != null;
              // 字符参数
              if (val.ParamType == "System.String")
              {
                paramValue = string.IsNullOrEmpty(val.Value) ? null : val.Value;
                tempParameters?.Add(val.Name, paramValue);
              }
              else if (val.ParamType.ToUpper().Contains("DATE") && isEnumerable == false)
              {
                //日期参数
                paramValue = string.IsNullOrEmpty(val.Value) ? null : (DateTime?)DateTime.Parse(val.Value);
                tempParameters?.Add(val.Name, paramValue);
              }
              else if (isEnumerable == true)
              {
                // 枚举参数

                #region 处理枚举值

                if (val.ParamType.ToUpper().Contains("STRING"))
                {
                  string[] avary = string.IsNullOrEmpty(val.Value)
                    ? null
                    : JsonConvert.DeserializeObject<string[]>(val.Value);
                  tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
                }
                else if (val.ParamType.ToUpper().Contains("DATE"))
                {
                  DateTime[] avary = string.IsNullOrEmpty(val.Value)
                    ? null
                    : JsonConvert.DeserializeObject<DateTime[]>(val.Value);
                  tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
                }
                else
                {
                  double[] avary = string.IsNullOrEmpty(val.Value)
                    ? null
                    : JsonConvert.DeserializeObject<double[]>(val.Value);
                  tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
                }

                #endregion
              }
              else
              {
                // 数字参数
                paramValue = string.IsNullOrEmpty(val.Value) ? null : (double?)double.Parse(val.Value);
                tempParameters?.Add(val.Name, paramValue);
              }
            }

            #endregion

            executeCount += Connection.Execute(commandText, tempParameters, trans, commandTimeout, commandType);
          }

          trans.Commit();
        }
        catch
        {
          trans.Rollback();
          executeCount = 0;
          throw;
        }
        finally
        {
          Dispose();
        }
      }

      return executeCount;
    }

    /// <summary>
    /// 执行UserCommand
    /// </summary>
    /// <param name="command"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Execute(UserCommand command, int? commandTimeout = null, CommandType? commandType = null)
    {
      int executeCount = 0;

      if (command == null)
        throw new Exception("UserCommand cannot be empty");

      OpenConnection(this.Connection);

      try
      {
        var commandTemp = command;
        // 提取SQL參數
        string commandText = commandTemp.CommandText;
        DynamicParameters tempParameters = null;

        #region Extract SQL parameters to DynamicParameters

        var parameterList = commandTemp.Parameters;
        if (parameterList.Count > 0)
        {
          tempParameters = new DynamicParameters();

          #region Detecting parameter missing transmission

          string tempCommandText = commandText.ToString();
          // 按照调用时传递的参数擦除当前SQL对应的参数栏位：如 Name=:Name 变成 Name=Name
          foreach (var item in parameterList)
          {
            tempCommandText = tempCommandText.Replace($":{item.Name}", item.Name);
          }

          // 检测擦除参数栏位之后，SQL里面是否还残留 “:”
          tempCommandText = tempCommandText.Replace(" ", "").ToUpper(); //去除空格
          if (tempCommandText.Contains("=:") || tempCommandText.Contains("IN:"))
          {
            throw new Exception("Parameterized SQL must upload all required parameters");
          }

          #endregion
        }

        foreach (var item in parameterList)
        {
          UserParameter val = item;
          object paramValue = null;

          // 判断参数是否枚举值
          bool isEnumerable = Type.GetType(val.ParamType)?.GetInterface("System.Collections.IEnumerable") != null;
          // 字符参数
          if (val.ParamType == "System.String")
          {
            paramValue = string.IsNullOrEmpty(val.Value) ? null : val.Value;
            tempParameters?.Add(val.Name, paramValue);
          }
          else if (val.ParamType.ToUpper().Contains("DATE") && isEnumerable == false)
          {
            //日期参数
            paramValue = string.IsNullOrEmpty(val.Value) ? null : (DateTime?)DateTime.Parse(val.Value);
            tempParameters?.Add(val.Name, paramValue);
          }
          else if (isEnumerable == true)
          {
            // 枚举参数

            #region 处理枚举值

            if (val.ParamType.ToUpper().Contains("STRING"))
            {
              string[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<string[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }
            else if (val.ParamType.ToUpper().Contains("DATE"))
            {
              DateTime[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<DateTime[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }
            else
            {
              double[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<double[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }

            #endregion
          }
          else
          {
            // 数字参数
            paramValue = string.IsNullOrEmpty(val.Value) ? null : (double?)double.Parse(val.Value);
            tempParameters?.Add(val.Name, paramValue);
          }
        }

        #endregion

        executeCount += Connection.Execute(commandText, tempParameters, null, commandTimeout, commandType);
      }
      catch
      {
        executeCount = 0;
        throw;
      }
      finally
      {
        Dispose();
      }

      return executeCount;
    }

    /// <summary>
    /// 执行UserCommand
    /// </summary>
    /// <param name="command"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<int> ExecuteAsync(UserCommand command, int? commandTimeout = null,
      CommandType? commandType = null)
    {
      int executeCount = 0;

      if (command == null)
        throw new Exception("UserCommand cannot be empty");

      OpenConnection(this.Connection);

      try
      {
        var commandTemp = command;
        // 提取SQL參數
        string commandText = commandTemp.CommandText;
        DynamicParameters tempParameters = null;

        #region Extract SQL parameters to DynamicParameters

        var parameterList = commandTemp.Parameters;
        if (parameterList.Count > 0)
        {
          tempParameters = new DynamicParameters();

          #region Detecting parameter missing transmission

          string tempCommandText = commandText.ToString();
          // 按照调用时传递的参数擦除当前SQL对应的参数栏位：如 Name=:Name 变成 Name=Name
          foreach (var item in parameterList)
          {
            tempCommandText = tempCommandText.Replace($":{item.Name}", item.Name);
          }

          // 检测擦除参数栏位之后，SQL里面是否还残留 “:”
          tempCommandText = tempCommandText.Replace(" ", "").ToUpper(); //去除空格
          if (tempCommandText.Contains("=:") || tempCommandText.Contains("IN:"))
          {
            throw new Exception("Parameterized SQL must upload all required parameters");
          }

          #endregion
        }

        foreach (var item in parameterList)
        {
          UserParameter val = item;
          object paramValue = null;

          // 判断参数是否枚举值
          bool isEnumerable = Type.GetType(val.ParamType)?.GetInterface("System.Collections.IEnumerable") != null;
          // 字符参数
          if (val.ParamType == "System.String")
          {
            paramValue = string.IsNullOrEmpty(val.Value) ? null : val.Value;
            tempParameters?.Add(val.Name, paramValue);
          }
          else if (val.ParamType.ToUpper().Contains("DATE") && isEnumerable == false)
          {
            //日期参数
            paramValue = string.IsNullOrEmpty(val.Value) ? null : (DateTime?)DateTime.Parse(val.Value);
            tempParameters?.Add(val.Name, paramValue);
          }
          else if (isEnumerable == true)
          {
            // 枚举参数

            #region 处理枚举值

            if (val.ParamType.ToUpper().Contains("STRING"))
            {
              string[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<string[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }
            else if (val.ParamType.ToUpper().Contains("DATE"))
            {
              DateTime[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<DateTime[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }
            else
            {
              double[] avary = string.IsNullOrEmpty(val.Value)
                ? null
                : JsonConvert.DeserializeObject<double[]>(val.Value);
              tempParameters?.AddDynamicParams(new { IsEnumerableParameter = avary });
            }

            #endregion
          }
          else
          {
            // 数字参数
            paramValue = string.IsNullOrEmpty(val.Value) ? null : (double?)double.Parse(val.Value);
            tempParameters?.Add(val.Name, paramValue);
          }
        }

        #endregion

        executeCount = await Connection.ExecuteAsync(commandText, tempParameters, null, commandTimeout, commandType);
      }
      catch
      {
        executeCount = 0;
        throw;
      }
      finally
      {
        Dispose();
      }

      return executeCount;
    }

    /// <summary>
    /// 查询  
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public IEnumerable<TEntity> Query(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.Query<TEntity>(sql, param, transaction, buffered, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 查询
    /// </summary>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> QueryAsync(string sql, object param = null,
      IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 查询并返回指定对象
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 查询并返回指定对象
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 查询并返回第一个对象  
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 查询并返回第一个对象
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// 查询并返回第一个对象
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Async 查询并返回第一个对象 
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    /// <param name="sql">SQL脚本</param>
    /// <param name="param">SQL执行参数对象</param>
    /// <param name="transaction">操作的事务对象</param>
    /// <param name="commandTimeout">执行的超时时间</param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public IEnumerable<TEntity> QueryIn(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return Connection.Query<TEntity>(sql, param, transaction, buffered, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TEntity>> QueryAsyncIn(string sql, object param = null,
      IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return await Connection.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="buffered"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public IEnumerable<T> QueryIn<T>(string sql, object param = null, IDbTransaction transaction = null,
      bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> QueryAsyncIn<T>(string sql, object param = null,
      IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
      where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return await Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryFirstIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public T QueryFirstIn<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return Connection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryFirstAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return await Connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryFirstOrDefaultIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public T QueryFirstOrDefaultIn<T>(string sql, object param = null, IDbTransaction transaction = null,
      int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// QueryFirstOrDefaultAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> QueryFirstOrDefaultAsyncIn<T>(string sql, object param = null,
      IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
      try
      {
        OpenConnection(this.Connection);

        ListExtensions.ParamIsListEmpty(ref param);
        return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
      }
      catch
      {
        throw;
      }
      finally
      {
        Dispose();
      }
    }

    /// <summary>
    /// Connection Colse
    /// </summary>
    public void Dispose()
    {
      if (Connection.State != ConnectionState.Closed)
        Connection.Close();

      if (Connection.State == ConnectionState.Open)
        Connection.Close();
    }
  }
}