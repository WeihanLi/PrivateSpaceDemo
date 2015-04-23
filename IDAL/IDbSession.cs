using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    /// 数据仓储 大接口
    /// </summary>
    public interface IDbSession
    {
        IUser IUser { get; set; }
        IMood IMood { get; set; }
        IDiary IDiary { get; set; }

        int SaveChanges();
    }
}
