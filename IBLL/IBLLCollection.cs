using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public interface IUser :IBaseBLL<Model.User>
    {
        Model.LoginState Login(Model.ViewModel.LoginViewModel model);

        bool Regist(Model.ViewModel.RegisterViewModel model);

        bool ActivateAccount(Model.User u);
    }

    public interface IMood :IBaseBLL<Model.Mood>{ }

    public interface IDiary : IBaseBLL<Model.Diary> { }

}
