using Dapper;
using Dapper.Contrib.Extensions;
using DapperHelper.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
    /// <summary>
    /// Dapper数据操作基类
    /// </summary>
    /// <typeparam name="TEntity">仓储实体</typeparam>
    public class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        public DapperRepository()
        {
            this.Connection = DBFactory.CreateConnection();
        }

        public DapperRepository(DBProvider providers)
        {
            this.Connection = DBFactory.CreateConnection(providers);
        }

        public DapperRepository(IDbConnection dbConnection)
        {
            this.Connection = dbConnection;
        }
        public IDbConnection Connection
        {
            get; private set;
        }

        #region Connection基础操作

        /// <summary>
        /// 打开数据库连接,如果关闭或者连接中断,则打开连接
        /// </summary>
        public void OpenConnection(IDbConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            } else if (conn.State == ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
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
        /// 根据主键获取实体数据
        /// </summary>
        /// <typeparam name="TEntity">仓储实体</typeparam>
        /// <param name="key">主键值</param>
        /// <returns></returns>
        public virtual TEntity Get(object key)
        {
            return Get<TEntity>(key);
        }

        public TEntity Get(object key, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return this.Get<TEntity>(key, traction, commandTimeout);
        }

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <typeparam name="TEntity">仓储实体</typeparam>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(object key, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return await GetAsync<TEntity>(key);
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
            return Connection.Get<T>(key, traction, commandTimeout);
        }

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(object key, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return await Connection.GetAsync<T>(key, traction, commandTimeout);
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
        public DataTable Query_DataTable(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
            DataTable dt = new DataTable();
            dt.Load(d);
            return dt;
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
                            sqlParam.Value.ForEach(param =>
                            {
                                Connection.Execute(sql, param, trans);
                            });
                        } else
                        {
                            Connection.Execute(sql, trans);
                        }
                    }
                    trans.Commit();
                } catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// SQL集合批量（循环）执行
        /// <para>
        /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）；[-2]表示执行SQL集合，执行结束后，成功数不等于集合数（回退事务）
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        public int BatchExecutionForeach(List<string> list)
        {
            int ExecutionRow = 0;
            Connection.Open();
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
                        int ResultRow = this.Connection.Execute(sqlOriginal, transaction:transation);
                        if (ResultRow == 1)
                        {
                            ExecutionRow += ResultRow;
                        }
                    }
                    // 判断执行成功行数是否与SQL集合数一致，不一致表示有SQL在执行出现异常（出现异常进行事务回退）
                    if (ExecutionRow == list.Count)
                    {
                        transation.Commit();
                    } else
                    {
                        transation.Rollback();
                        ExecutionRow = -2;
                    }
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    ExecutionRow = -1;
                }
            }
            return ExecutionRow;
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
            int ExecutionRow = 1;
            Connection.Open();
            using (var transation = this.Connection.BeginTransaction())
            {
                try
                {
                    string sql = $@"begin
";
                    foreach (string item in list)
                    {
                        sql += item + "\r\n";
                    }
                    sql += "end;";

                    int result = Connection.Execute(sql, transaction: transation);
                    transation.Commit();
                    return ExecutionRow;
                } catch (Exception ex)
                {
                    transation.Rollback();
                    ExecutionRow = 0;
                }
            }
            return ExecutionRow;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="dynamicParameters">存储过程参数对象（包含input，output）</param>
        /// <returns>执行存储过程状态</returns>
        public int ExecuteProcedure(string ProcName, DynamicParameters dynamicParameters)
        {
            int ExecutionStatus = 1;
            Connection.Open();
            using (var transaction = this.Connection.BeginTransaction())
            {
                try
                {
                    int result = Connection.Execute(ProcName, dynamicParameters, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                    return ExecutionStatus;
                } 
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExecutionStatus = 0;
                }
            }
            return ExecutionStatus;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return GetAll<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(IDbTransaction traction = null, int? commandTimeout = null)
        {
            return this.GetAll<TEntity>(traction, commandTimeout);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IDbTransaction traction = null, int? commandTimeout = null)
        {
            return await Connection.GetAllAsync<TEntity>(traction, commandTimeout);
        }

        public IEnumerable<T> GetAll<T>(IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return Connection.GetAll<T>(traction, commandTimeout);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return await Connection.GetAllAsync<T>(traction, commandTimeout);
        }

        public bool Insert(TEntity entityToInsert)
        {
            return this.Connection.Insert<TEntity>(entityToInsert) > 0;
        }

        public bool Insert(TEntity entityToInsert, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return this.Insert<TEntity>(entityToInsert, traction, commandTimeout) > 0;
        }

        public async Task<bool> InsertAsync(TEntity entityToInsert, IDbTransaction traction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null)
        {
            return await InsertAsync<TEntity>(entityToInsert, traction, commandTimeout) > 0;
        }

        public long Insert<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return Connection.Insert<T>(entityToInsert, traction, commandTimeout);
        }

        public async Task<long> InsertAsync<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null) where T : class, new()
        {
            return await Connection.InsertAsync<T>(entityToInsert, traction, commandTimeout, sqlAdapter);
        }

        public bool Update(TEntity entityToUpdate)
        {
            return this.Connection.Update<TEntity>(entityToUpdate);
        }

        public bool Update(TEntity entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return this.Update<TEntity>(entityToUpdate, traction, commandTimeout);
        }

        public async Task<bool> UpdateAsync(TEntity entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return await UpdateAsync<TEntity>(entityToUpdate);
        }

        public bool Update<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return Connection.Update<T>(entityToUpdate, traction, commandTimeout);
        }

        public async Task<bool> UpdateAsync<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return await Connection.UpdateAsync<T>(entityToUpdate, traction, commandTimeout);
        }

        public bool Delete(TEntity entityToDelete, IDbTransaction traction = null, int? commandTimeout = null)
        {
            return this.Delete<TEntity>(entityToDelete, traction, commandTimeout);
        }

        public bool Delete(TEntity entityToDelete)
        {
            return this.Connection.Delete<TEntity>(entityToDelete);
        }

        public bool Delete<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return Connection.Delete(entityToDelete, traction, commandTimeout);
        }

        public async Task<bool> DeleteAsync<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new()
        {
            return await Connection.DeleteAsync<T>(entityToDelete, traction, commandTimeout);
        }

        public int Execute(string sql, object param = null)
        {
            return Connection.Execute(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            return await Connection.ExecuteAsync(sql, param);
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<TEntity> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Query<TEntity>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public async Task<IEnumerable<TEntity>> QueryAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Connection.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            return await Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            return Connection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            return await Connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        #region SQL IN 参数Empty Query方法

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
        public IEnumerable<TEntity> QueryIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            ListEx.ParamIsListEmpty(ref param);
            return Connection.Query<TEntity>(sql, param, transaction, buffered, commandTimeout, commandType);
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
        public async Task<IEnumerable<TEntity>> QueryAsyncIn(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ListEx.ParamIsListEmpty(ref param);
            return await Connection.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
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
        public IEnumerable<T> QueryIn<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            ListEx.ParamIsListEmpty(ref param);
            return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
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
        public async Task<IEnumerable<T>> QueryAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            ListEx.ParamIsListEmpty(ref param);
            return await Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
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
        public T QueryFirstIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            ListEx.ParamIsListEmpty(ref param);
            return Connection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
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
        public async Task<T> QueryFirstAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new()
        {
            ListEx.ParamIsListEmpty(ref param);
            return await Connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
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
        public T QueryFirstOrDefaultIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ListEx.ParamIsListEmpty(ref param);
            return Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
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
        public async Task<T> QueryFirstOrDefaultAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ListEx.ParamIsListEmpty(ref param);
            return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
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
        public DataTable Query_DataTableIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            ListEx.ParamIsListEmpty(ref param);
            IDataReader d = Connection.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
            DataTable dt = new DataTable();
            dt.Load(d);
            return dt;
        }

        #endregion

        /// <summary>
        /// Connection Colse
        /// </summary>
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
