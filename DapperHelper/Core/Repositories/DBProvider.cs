using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Core.Repositories
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBProvider
    {
        /// <summary>
        /// MES DB
        /// </summary>
        MESCon,
        /// <summary>
        /// 接口 DB
        /// </summary>
        InterfaceDB,
        /// <summary>
        /// MES log DB
        /// </summary>
        MESLogCon,
        /// <summary>
        /// MES Other DB
        /// </summary>
        MESOtherCon
    }
}
