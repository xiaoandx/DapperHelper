using System.Data;

namespace DapperHelpers.Core.Extensions
{
  /// <summary>
  ///  DataTable扩展方法
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.1</para>
  /// </remarks>
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