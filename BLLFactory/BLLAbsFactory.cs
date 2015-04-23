using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLLFactory
{
    /// <summary>
    /// Service（大BLL）层对应抽象工厂
    /// </summary>
    public abstract class BLLAbsFactory
    {
        /// <summary>
        /// 根据配置文件获取相应实体BLL工厂
        /// </summary>
        /// <returns>实体BLL工厂</returns>
        public static BLLAbsFactory GetBLLFactory()
        {
            string BLLType = Common.ConfigurationHelper.AppSetting("BLLType");
            BLLAbsFactory BLLFac = null;
            switch (BLLType)
            {
                case "BLL":
                    BLLFac = new BLLFactory();
                    break;
                default:
                    break;
            }
            return BLLFac;
        }

        public static IBLLSession GetBLLSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IBLLSession iBLLSession = System.Runtime.Remoting.Messaging.CallContext.GetData(typeof(BLLAbsFactory).Name) as IBLLSession;
            if (iBLLSession == null)
            {
                //通过反射创建 IDbSession 
                //1.通过配置文件获取dll的位置
                string dllPath = Common.ConfigurationHelper.AppSetting("BLLSessionFactoryDLL");
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
                System.Runtime.Remoting.Messaging.CallContext.SetData(typeof(BLLAbsFactory).Name, iBLLSession);
            }
            return iBLLSession;
        }

        /// <summary>
        /// 获取User实体类
        /// </summary>
        /// <returns>IBLL.IUser 接口</returns>
        public abstract IBLL.IUser GetUser();
    }
}
