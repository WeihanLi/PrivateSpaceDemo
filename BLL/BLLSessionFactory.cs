using IBLL;
using System;
using System.Reflection;

namespace BLL
{
    public class BLLSessionFactory : IBLL.IBLLSessionFactory
    {
        public IBLLSession GetBLLSession()
        {
            //从当前线程中 获取 DBContext 数据仓储 对象
            IBLLSession iBLLSession = System.Runtime.Remoting.Messaging.CallContext.GetData(typeof(BLLSessionFactory).Name) as IBLLSession;
            if (iBLLSession == null)
            {
                iBLLSession = new BLLSession();
                System.Runtime.Remoting.Messaging.CallContext.SetData(typeof(BLLSessionFactory).Name, iBLLSession);
            }
            return iBLLSession;            
        }
    }
}
