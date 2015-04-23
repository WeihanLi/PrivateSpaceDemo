using System;
using IBLL;
using System.Reflection;

namespace BLLFactory
{
    /// <summary>
    /// 创建BLL中实体类型
    /// </summary>
    public class BLLFactory : BLLAbsFactory
    {

        /// <summary>
        /// 获取BLL中User实体
        /// </summary>
        /// <returns></returns>
        public override IUser GetUser()
        {
            return new BLL.User();
        }
    }
}
