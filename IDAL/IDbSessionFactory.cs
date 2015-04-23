using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// 数据仓储工厂
    /// </summary>
    public interface IDbSessionFactory
    {
        IDbSession GetDbSession();
    }
}
