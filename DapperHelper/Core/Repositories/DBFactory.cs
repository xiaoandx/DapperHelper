using DapperHelper.Core.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DapperHelper.Core.Repositories
{
    public class DBFactory
    {
        public static IDbConnection CreateConnection()
        {
            return CreateConnection(DBProvider.MESCon);
        }

        public static IDbConnection CreateConnection(DBProvider providers)
        {
            IDbConnection connection = null;
            //获取连接字符串
            string connectionString = TryGetConnectionString(providers);

            connection = new OracleConnection(connectionString);

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


        public static string TryGetConnectionString(DBProvider providers)
        {
            var connString = string.Empty;
            try
            {
                connString = ConfigManager.GetAppSettingString(providers.ToString());
                //ConfigurationManager.ConnectionStrings[providers.ToString()].ConnectionString;

            } catch
            {
                //TODO:: ConnectionStrings为空处理
            }
            return connString;
        }
    }
}
