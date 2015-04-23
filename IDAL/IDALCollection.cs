using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IUser : IBaseDAL<Model.User>
    {
        Model.LoginState Login(Model.User model);

        bool Regist(Model.ViewModel.RegisterViewModel model);

        bool ActivateAccount(Model.User u);
    }


    public interface IMood : IBaseDAL<Model.Mood> { }

    public interface IDiary : IBaseDAL<Model.Diary> { }
}
