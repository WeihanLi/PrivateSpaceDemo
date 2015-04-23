using System.Runtime.Remoting.Messaging;

namespace DALMSSQL
{
    public class DbSessionFactory : IDAL.IDbSessionFactory
    {
        public IDAL.IDbSession GetDbSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IDAL.IDbSession dbSesion = CallContext.GetData(typeof(DbSessionFactory).Name) as IDAL.IDbSession;
            if (dbSesion == null)
            {
                dbSesion = new DbSession();
                CallContext.SetData(typeof(DbSessionFactory).Name, dbSesion);
            }
            return dbSesion;
        }
    }
}
