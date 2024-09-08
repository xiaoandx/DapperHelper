using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Core.Extensions
{
    /// <summary>
    ///  DataTable扩展方法
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// 获取 DataTable 的第一行数据。
        /// </summary>
        /// <param name="table">需要操作的 DataTable。</param>
        /// <returns>DataTable 的第一行数据,如果 DataTable 为空或没有行，则返回 null。</returns>
        public static DataRow FirstRow(this DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return null;
            }
            return table.Rows[0];
        }
    }
}
