using System;
using Model;

namespace DALMSSQL
{
    public class User : BaseDAL<Model.User>, IDAL.IUser
    {
        public bool ActivateAccount(Model.User u)
        {
            try
            {
                u.AuthCode = null;
                if (Modify(u, "AuthCode")>0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="u">登录视图模型实体</param>
        /// <returns>登录状态</returns>
        public Model.LoginState Login(Model.User u)
        {
            //判断用户是否经过验证，如果没有，返回 需要验证 Model.LoginState.RequireVerification
            // return Model.LoginState.RequireVerification
            if (Exist(user => user.Mail == u.Mail && user.Password == u.Password))
            {
                return Model.LoginState.Success;
            }
            return Model.LoginState.Failure;
        }
        
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="u">用户实体</param>
        /// <returns>是否注册成功</returns>
        public bool Regist(Model.ViewModel.RegisterViewModel u)
        {
            if (!Exist(user => user.Mail == u.Email))
            {
                Model.User m = new Model.User() { Username = u.Name, Password = u.Password, Mail = u.Email, RegTime = System.DateTime.Now, AuthCode = System.Guid.NewGuid().ToString() };
                if (Add(m)==1)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Mood : BaseDAL<Model.Mood>, IDAL.IMood { }

    public class Diary : BaseDAL<Model.Diary>, IDAL.IDiary { }
}
