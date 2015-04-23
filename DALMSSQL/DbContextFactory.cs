using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace DALMSSQL
{
    class DbContextFactory : IDAL.IDbContextFactory
    {
        public DbContext GetDbContext()
        {
            //从当前线程中 获取 EF上下文对象
            DbContext dbContext = CallContext.GetData(typeof(DbContextFactory).Name) as DbContext;
            if (dbContext == null)
            {
                dbContext = new Model.PrivateEntity();
                dbContext.Configuration.ValidateOnSaveEnabled = false;
                //将新创建的 ef上下文对象 存入线程
                CallContext.SetData(typeof(DbContextFactory).Name, dbContext);
            }
            return dbContext;
        }
    }
}
