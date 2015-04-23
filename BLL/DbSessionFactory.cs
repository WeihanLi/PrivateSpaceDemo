using IDAL;
using System;
using System.Reflection;

namespace BLL
{
    public class DbSessionFactory : IDAL.IDbSessionFactory
    {
        public IDbSession GetDbSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IDbSession dbSession = System.Runtime.Remoting.Messaging.CallContext.GetData(typeof(DbSessionFactory).Name) as IDbSession;
            if (dbSession == null)
            {
                //通过反射创建 IDbSession 
                //1.通过配置文件获取dll的位置
                string dllPath = Common.ConfigurationHelper.AppSettingMapPath("DbSessionFactoryDLL");
                //2.通过配置文件获取 DbSessionFactory 的全名称
                string factoryType = Common.ConfigurationHelper.AppSetting("DbSessionFactoryType");
                //3.通过反射创建实例
                //3.1加载程序集
                Assembly dll = Assembly.LoadFrom(dllPath);
                //3.2获得DbSessionFactory 的 Type
                System.Type tFactory = dll.GetType(factoryType);
                //3.3反射创建 IDAL.IDbSessionFactory 实体对象
                IDAL.IDbSessionFactory sessionFac = Activator.CreateInstance(tFactory) as IDAL.IDbSessionFactory;
                dbSession = sessionFac.GetDbSession();
                System.Runtime.Remoting.Messaging.CallContext.SetData(typeof(DbSessionFactory).Name, dbSession);
            }
            return dbSession;
        }
    }
}
