using System.IO;
using System.Web.Mvc;

namespace PrivateSpace.API
{
    public class MemberController :Controller
    {
        private static Model.FormatModel.JsonMsgModel json = null;
        private static Common.LogHelper logger = new Common.LogHelper(typeof(MemberController));
        // 测试用
        //public ActionResult Login()
        //{
        //    return Content("我是登录页面！");
        //}

        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="model">登录实体</param>
        /// <returns>登录结果的Json数据</returns>
        [HttpPost]
        public JsonResult Login(Model.ViewModel.LoginViewModel model)
        {
            if (model==null)
            {
                model = Helper.OperateContext.RequestParams<Model.ViewModel.LoginViewModel>();;
            }
            string content = null;
            if (ModelState.IsValid)
            {
                switch (Helper.OperateContext.Current.Login(model))
                {
                    case Model.LoginState.Success:
                        //跳转至管理页面
                        content = "Login Success";
                        break;
                    case Model.LoginState.Failure:
                        content = "Login Fail!";
                        break;
                    case Model.LoginState.RequireVerification:
                        content = "Your account RequireVerification!";
                        break;
                    case Model.LoginState.LockedOut:
                        content = "Your account has LockedOut!";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                content="输入数据格式有误";
            }
            json = new Model.FormatModel.JsonMsgModel() { Status = Model.FormatModel.JsonResultStatus.ok, Data = content, Msg = "success" };
            return Json(json);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model">注册信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(Model.ViewModel.RegisterViewModel model)
        {
            if (model==null)
            {
                model= Helper.OperateContext.RequestParams<Model.ViewModel.RegisterViewModel>();
            }

            if (ModelState.IsValid)
            {
                if (Helper.OperateContext.Current.Register(model))
                {
                    json = new Model.FormatModel.JsonMsgModel() { Data = model, Status = Model.FormatModel.JsonResultStatus.ok, Msg = "success" };
                }
                else
                {
                    json = new Model.FormatModel.JsonMsgModel() { Data = model, Status = Model.FormatModel.JsonResultStatus.error, Msg = "Fail" };
                }
            }
            return Json(json);
        }

        /// <summary>
        /// 登出账户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogOut()
        {
            //Session["Member"] = null;
            //return RedirectToAction("Login");
            return null;
        }
    }
}
