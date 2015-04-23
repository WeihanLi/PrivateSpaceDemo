using System;
using Model;
using Model.ViewModel;

namespace BLL
{
    public class User : BaseBLL<Model.User>,IBLL.IUser
    {
        IDAL.IUser iUser = null;

        public bool ActivateAccount(Model.User u)
        {
            return iUser.ActivateAccount(u);
        }

        public Model.LoginState Login(LoginViewModel model)
        {
            //用户密码 加密 MD5++
            //获取加密后密码//将密码设置为加密后密码
            Model.User u = new Model.User() { Mail = model.Email, Password = Common.Helper.GetMD5(model.Password) };
            return iUser.Login(u);
        }

        public bool Regist(RegisterViewModel model)
        {
            //用户密码 加密 暂时使用MD5
            model.Password = Common.Helper.GetMD5(model.Password);
            return iUser.Regist(model);
        }

        public override void SetDal()
        {
            IDal = DbSession.IUser;
            iUser = IDal as IDAL.IUser; 
        }
        
    }

    public class Mood : BaseBLL<Model.Mood>, IBLL.IMood
    {
        public override void SetDal()
        {
            IDal = DbSession.IMood;
        }
    }

    public class Diary : BaseBLL<Model.Diary>, IBLL.IDiary
    {
        public override void SetDal()
        {
            IDal = DbSession.IDiary;
        }
    }
}
