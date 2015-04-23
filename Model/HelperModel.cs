using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LoginState { Success, Failure, RequireVerification, LockedOut }
}
