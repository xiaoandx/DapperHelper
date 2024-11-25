using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperHelper.Core.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <remarks>
    /// Author：Willis
    /// <para>Version: v1.6.0.0</para>
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class, new()
    {

        #region Base

        /// <summary>
        /// 根据表主键获取数据
        /// </summary>
        /// <param name="key">表主键</param>
        /// <returns></returns>
        TEntity Get(object key);

        /// <summary>
        /// 获取表所有数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entityToInsert">待插入表实体类</param>
        /// <returns></returns>
        bool Insert(TEntity entityToInsert);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entityToUpdate">待更新表实体类</param>
        /// <returns></returns>
        bool Update(TEntity entityToUpdate);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="entityToDelete">待删除表实体类</param>
        /// <returns></returns>
        bool Delete(TEntity entityToDelete);

        #endregion

        #region Get

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        TEntity Get(object key, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(object key, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        T Get<T>(object key, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 根据主键获取实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键值</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(object key, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region Insert

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entityToInsert">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>成功插入条数</returns>
        bool Insert(TEntity entityToInsert, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entityToInsert">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="sqlAdapter"></param>
        /// <returns>成功插入条数</returns>
        Task<bool> InsertAsync(TEntity entityToInsert, IDbTransaction traction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToInsert">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>成功插入条数</returns>
        long Insert<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToInsert">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="sqlAdapter"></param>
        /// <returns>成功插入条数</returns>
        Task<long> InsertAsync<T>(T entityToInsert, IDbTransaction traction = null, int? commandTimeout = null, ISqlAdapter sqlAdapter = null) where T : class, new();

        #endregion

        #region Update

        /// <summary>
        /// 修改实体数据
        /// </summary>
        /// <param name="entityToUpdate">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否修改成功</returns>
        bool Update(TEntity entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 修改实体数据
        /// </summary>
        /// <param name="entityToUpdate">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否修改成功</returns>
        Task<bool> UpdateAsync(TEntity entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 修改实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToUpdate">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否修改成功</returns>
        bool Update<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 修改实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToUpdate">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否修改成功</returns>
        Task<bool> UpdateAsync<T>(T entityToUpdate, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region Delete

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="entityToDelete">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否删除成功</returns>
        bool Delete(TEntity entityToDelete, IDbTransaction traction = null, int? commandTimeout = null);

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToDelete">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否删除成功</returns>
        bool Delete<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityToDelete">实体对象</param>
        /// <param name="traction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync<T>(T entityToDelete, IDbTransaction traction = null, int? commandTimeout = null) where T : class, new();

        #endregion

        #region Execute

        /// <summary>
        /// 执行sql语句,用于增删改查
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <returns>返回执行sql后受影响的行数</returns>
        int Execute(string sql, object param = null);

        /// <summary>
        /// 执行sql语句,用于增删改查
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <returns>返回执行sql后受影响的行数</returns>
        Task<int> ExecuteAsync(string sql, object param = null);

        /// <summary>
        /// 执行sql命令,用于增删改查
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回执行sql后受影响的行数</returns>
        int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 执行sql命令,用于增删改查
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回执行sql后受影响的行数</returns>
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        #endregion

        #region Query

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        IEnumerable<TEntity> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        Task<IEnumerable<TEntity>> QueryAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果则抛出异常
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果则抛出异常
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        Task<T> QueryFirstAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果返回默认值
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果返回默认值
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// <para>QueryIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        IEnumerable<TEntity> QueryIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// <para>QueryAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        Task<IEnumerable<TEntity>> QueryAsyncIn(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// <para>QueryIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        IEnumerable<T> QueryIn<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 查询数据,返回指定数据类型
        /// <para>QueryAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回指定数据类型的数据序列</returns>
        Task<IEnumerable<T>> QueryAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果则抛出异常
        /// <para>QueryFirstIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        T QueryFirstIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果则抛出异常
        /// <para>QueryFirstIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        Task<T> QueryFirstAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class, new();

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果返回默认值
        /// <para>QueryFirstOrDefaultIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        T QueryFirstOrDefaultIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 执行查询并将第一个结果映射到指定类型对象,无结果返回默认值
        /// <para>QueryFirstOrDefaultAsyncIn 该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零</para>
        /// </summary>
        /// <typeparam name="T">指定类型对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回查询结果集的第一个结果</returns>
        Task<T> QueryFirstOrDefaultAsyncIn<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        #endregion

        #region DataTable / DataRow / String
        /// <summary>
        /// Dapper 执行SQL返回DataTable类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataTable Query_DataTable(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Query_DataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataTable Query_DataTableIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Dapper 执行SQL返回DataTable类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataTable QueryDataTable(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// QueryDataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataTable QueryDataTableIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Dapper 执行SQL返回DataTable第First行数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataRow Query_DataTableFirstRow(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Query_DataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataRow Query_DataTableInFirstRow(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Dapper 执行SQL返回DataTable类型
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataRow QueryDataTableFirstRow(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// QueryDataTableIn 执行SQL返回DataTable类型,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        DataRow QueryDataTableInFirstRow(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// QueryString 执行SQL返回string类型的單個數據
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        string QueryString(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// QueryStringIn 执行SQL返回String类型的單個數據,该函数会对入参匿名对象param进行List类型检索，当包含List参数集合时避免ListCount等于零
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数对象</param>
        /// <param name="transaction">此次操作的事务对象</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">执行的超时时间</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        string QueryStringIn(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        #endregion

        #region Batch
        /// <summary>
        /// (事务) 单条SQL，多次执行（每次执行传递数据不同），该方法执行SQL采用事务控制
        /// <para>
        /// 默认调用MesCon连接字符串
        /// </para>
        /// </summary>
        /// <param name="sqlParamDir"></param>
        /// <exception cref="Exception"></exception>
        void DapperExecuteBatch(Dictionary<string, List<object>> sqlParamDir);

        /// <summary>
        /// (事务) 单条SQL，多次执行（每次执行传递数据不同），该方法执行SQL采用事务控制
        /// <para>
        /// 默认调用MesCon连接字符串
        /// </para>
        /// <para>
        /// 该方法执行出现错误，可以通过Out返回错误信息
        /// </para>
        /// </summary>
        /// <param name="sqlParamDir"></param>
        /// <param name="errorMsg">out 錯誤信息</param>
        /// <exception cref="Exception"></exception>
        void DapperExecuteBatch(Dictionary<string, List<object>> sqlParamDir, out string errorMsg);

        /// <summary>
        /// (事务) SQL集合批量（循环）执行
        /// <para>
        /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）；
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        int BatchExecutionForeach(List<string> list);

        /// <summary>
        /// (事务) SQL集合批量（循环）执行
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
        int BatchExecutionForeach(List<string> list, out string errorMsg);

        /// <summary>
        /// (事务) SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
        /// <para>
        /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        int BatchExecutionBeginEnd(List<string> list);

        /// <summary>
        /// (事务) SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
        /// <para>
        /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误
        /// </para>
        /// <para>
        /// 该方法执行出现错误，可以通过Out返回错误信息
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <param name="errorMsg">异常错误信息</param>
        /// <returns>执行状态</returns>
        int BatchExecutionBeginEnd(List<string> list, out string errorMsg);

        /// <summary>
        /// (无事务) SQL集合批量（循环）执行
        /// <para>
        /// return [Row] 表示执行成功返回受影响行数；执行出现异常将转抛异常
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行完成受影响的行数</returns>
        int BatchExecutionForeachNTS(List<string> list);

        /// <summary>
        /// (无事务) SQL集合拼接为字符串采用BeginEnd执行
        /// <para>
        /// return [1] 表示执行成功；执行异常将抛出异常信息
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        int BatchExecutionBeginEndNTS(List<string> list);

        /// <summary>
        /// (事务) SQL集合批量（循环）执行
        /// <para>
        /// return [Row] 表示执行成功返回受影响行数；[-1] 表示执行异常（回退异常）
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        Task<int> BatchExecutionForeachAsync(List<string> list);

        /// <summary>
        /// (事务) SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
        /// <para>
        /// return [1] 表示执行成功（提交事务）；[0] 表示执行异常（回退异常），出现异常可能为SQL集合中部分SQL语句错误
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        Task<int> BatchExecutionBeginEndAsync(List<string> list);

        /// <summary>
        /// (无事务) SQL集合批量（循环）执行
        /// <para>
        /// return [Row] 表示执行成功返回受影响行数；执行出现异常将转抛异常
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行完成受影响的行数</returns>
        Task<int> BatchExecutionForeachAsyncNTS(List<string> list);

        /// <summary>
        /// (无事务) SQL集合拼接为字符串采用BeginEnd执行，SQL集合中的SQL必须以[;]结尾
        /// <para>
        /// return [1] 表示执行成功；执行异常将抛出异常信息
        /// </para>
        /// </summary>
        /// <param name="list">SQL集合</param>
        /// <returns>执行状态</returns>
        Task<int> BatchExecutionBeginEndAsyncNTS(List<string> list);

        /// <summary>
        /// (事务) ExecuteSqlListAsync,异步批量执行Sql List
        /// </summary>
        /// <param name="sqlList">SQLS对象集合</param>
        /// <returns></returns>
        Task ExecuteSqlListAsync(IEnumerable<string> sqlList);

        /// <summary>
        /// (事务) ExecuteSqlListAsyncResultRows,异步批量执行Sql List
        /// </summary>
        /// <param name="sqlList">SQLS对象集合</param>
        /// <returns>执行结果</returns>
        Task<int> ExecuteSqlListAsyncResultRows(IEnumerable<string> sqlList);

        /// <summary>
        /// (无事务) ExecuteSqlListAsync,异步批量执行Sql List，取消事务管理
        /// </summary>
        /// <param name="sqlList">SQLS对象集合</param>
        /// <returns>执行异常会抛出对应错误异常</returns>
        Task ExecuteSqlListAsyncNTS(IEnumerable<string> sqlList);

        /// <summary>
        /// (无事务) ExecuteSqlListAsyncResultRowsNTS,异步批量执行Sql List，取消事务管理，执行成功返回受影响的行数
        /// </summary>
        /// <param name="sqlList">SQLS对象集合</param>
        /// <returns>执行成功返回受影响的行数，执行异常会抛出对应错误异常</returns>
        Task<int> ExecuteSqlListAsyncResultRowsNTS(IEnumerable<string> sqlList);
        #endregion

        #region Procedure
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="dynamicParameters">存储过程参数对象（包含input，output）</param>
        /// <returns>执行存储过程状态</returns>
        int ExecuteProcedure(string ProcName, DynamicParameters dynamicParameters);

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
        int ExecuteProcedure(string ProcName, DynamicParameters dynamicParameters, out string errorMsg);
        #endregion

        /// <summary>
        /// 释放对象方法
        /// </summary>
        void Dispose();
    }
}
