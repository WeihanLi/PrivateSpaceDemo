using IBLL;
using System;
using System.Reflection;

namespace PrivateSpace.Helper
{
    /// <summary>
    /// BLL 仓储工厂
    /// </summary>
    internal class BLLSessionFactory : IBLL.IDbSessionFactory
    {
        /// <summary>
        /// 获取 BLL 业务逻辑层 大接口
        /// </summary>
        /// <returns></returns>
        public IBLLSession GetBLLSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IBLLSession iBLLSession = System.Runtime.Remoting.Messaging.CallContext.GetData(typeof(BLLSessionFactory).Name) as IBLLSession;
            if (iBLLSession == null)
            {
                //通过反射创建 IDbSession 
                //1.通过配置文件获取dll的位置
                string dllPath = Common.ConfigurationHelper.AppSettingMapPath("BLLSessionFactoryDLL");
                //string dllPath = System.Configuration.ConfigurationSettings.AppSettings["BLLSessionFactoryDLL"].ToString();
                //2.通过配置文件获取 DbSessionFactory 的全名称
                string factoryType = Common.ConfigurationHelper.AppSetting("BLLSessionFactoryType");
                //3.通过反射创建实例
                //3.1加载程序集
                Assembly dll = Assembly.LoadFrom(dllPath);
                //3.2获得DbSessionFactory 的 Type
                Type tFactory = dll.GetType(factoryType);
                //3.3反射创建 IBLLSessionFactory 实体对象
                IBLLSessionFactory BLLSession = Activator.CreateInstance(tFactory) as IBLLSessionFactory;
                iBLLSession = BLLSession.GetBLLSession();
                System.Runtime.Remoting.Messaging.CallContext.SetData(typeof(BLLSessionFactory).Name, iBLLSession);
            }
            return iBLLSession;
        }
    }
}
