using System;
using System.Web.Mvc;

namespace PrivateSpace.Logic
{
    public class MemberController : Controller
    {
        #region Login
        public ActionResult Login()
        {
            try
            {
                if (HttpContext.Request.Cookies["uid"] == null || HttpContext.Request.Cookies["pwd"] == null)
                {
                    return View();
                }
                else
                {
                    //从cookie 中获取 用户登录邮箱 及 密码（从cookie中获取需要解密）
                    Model.ViewModel.LoginViewModel login = new Model.ViewModel.LoginViewModel() { Email = Common.Helper.DecryptUserInfo(HttpContext.Request.Cookies["uid"].Value), Password = Common.Helper.DecryptUserInfo(HttpContext.Request.Cookies["pwd"].Value), RememberMe = true };
                    //判断用户名密码是否匹配
                    if (Helper.OperateContext.Current.Login(login) == Model.LoginState.Success)
                    {
                        //跳转至管理页面
                        return RedirectToAction("Index", "Manage");
                    }
                    //cookie被篡改，要求重新登录
                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(ChatController)).Error(ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(Model.ViewModel.LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string content = null;
                    switch (Helper.OperateContext.Current.Login(model))
                    {
                        case Model.LoginState.Success:
                            //跳转至管理页面
                            return RedirectToAction("Index", "Manage");
                        //break;
                        case Model.LoginState.Failure:
                            content = "Login Fail!";
                            break;
                        case Model.LoginState.RequireVerification:
                            content = "Your account needs to require Verification!";
                            break;
                        case Model.LoginState.LockedOut:
                            content = "Your account has LockedOut!";
                            break;
                        default:
                            break;
                    }
                    return Content("<h4>" + content + "</h4>");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(MemberController)).Error(ex.Message+ex.StackTrace);
                return View();
            }
        }
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Model.ViewModel.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Helper.OperateContext.Current.Register(model))
                    {
                        //return RedirectToAction("Index", "Manage");
                        //需要激活账户时~~~~~
                        return Content("<h4>快要完成了，系统已发送一封账户激活邮件到您的邮箱，请登录您的邮箱，激活您的账户</h4>");
                    }
                    else
                    {
                        //
                        return Content("<script>alert('账户注册发生错误。。。，请稍后重试')</script>");
                    }
                }
                catch (Exception ex)
                {
                    new Common.LogHelper(typeof(MemberController)).Error(ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region LogOut
        /// <summary>
        /// 登出账户
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            Session["Member"] = null;
            HttpContext.Response.Cookies["uid"].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 验证码
        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="code">用户输入的验证码</param>
        /// <returns>是否正确，<code>true</code>则正确，<code>false</code>不正确</returns>
        public ActionResult ValidCode(string ValidCode)
        {
            bool isValid = false;
            if (ValidCode.ToUpper() == HttpContext.Session["ValidCode"].ToString().ToUpper())
            {
                isValid = true;
            }
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public void GetCode()
        {
            Common.Helper.GenerateValidCode(HttpContext);
        }
        #endregion

        #region 检查邮箱是否重复验证,邮箱是否存在
        /// <summary>
        /// 检查邮箱是否重复验证
        /// </summary>
        /// <param name="Email">邮箱</param>
        /// <returns>是否重复，<code>true</code>重复</returns>
        public JsonResult CheckMail(string Email)
        {
            bool isValid = false;
            if (!Helper.OperateContext.Current.ExistMail(Email))
            {
                isValid = true;
            }
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExistMail(string Email)
        {
            return Json(Helper.OperateContext.Current.ExistMail(Email), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 激活注册账户
        /// <summary>
        /// 激活注册账户
        /// </summary>
        /// <param name="Mail">邮箱</param>
        /// <param name="AuthCode">激活验证码</param>
        /// <returns></returns>
        public ActionResult ValidAuthCode(string Mail, string AuthCode)
        {
            try
            {
                Model.User m = Helper.OperateContext.Current.SelectMember(Mail);
                if (m.AuthCode == null)
                {
                    return Content("<script>alert('该验证码已失效');location.href='/Member/Login'</script>");
                }
                else if (m.AuthCode == AuthCode)
                {
                    Helper.OperateContext.Current.ActivateAccount(m);
                    return Content("<script>alert('账户激活成功，请重新登录');location.href='/Member/Login'</script>");
                }
                else
                {
                    return Content("<script>alert('无效的链接')</script>");
                }
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(MemberController)).Error(ex.Message);
            }
            return null;
        } 
        #endregion

        #region ForgotPassword

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Model.ViewModel.ForgotPasswordViewModel model)
        {
            Model.User m = Helper.OperateContext.Current.SelectMember(model.Email);
            m.AuthCode = Guid.NewGuid().ToString();
            if (Helper.OperateContext.Current.UpdateUserInfo(m, "AuthCode") && Helper.OperateContext.Current.SendResetPwdMail(m))
            {
                return Content("<h4>快要完成了，系统已发送一封密码重置邮件到您的邮箱，请登录您的邮箱，重置您的账户密码</h4>");
            }
            else
            {
                return Content("<h4>发生异常，请稍后重试</h4>");
            }
        }
        #endregion

        #region ResetPassword

        [HttpGet]
        public ActionResult ResetPassword(string Email, string ValidCode)
        {
            Model.User m = Helper.OperateContext.Current.SelectMember(Email);
            Model.ViewModel.ResetPasswordViewModel model = null;
            if (m.AuthCode == ValidCode)
            {
                model = new Model.ViewModel.ResetPasswordViewModel() { Email = Email, Code = ValidCode };
                return View(model);
            }
            else
            {
                return Content("<h4>无效的链接！</h4>");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(Model.ViewModel.ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Helper.OperateContext.Current.ResetPwd(model))
                {
                    return Content("<script>alert('密码已重置，请重新登录！');location.href='/Member/Login'</script>");
                }
            }
            return Content("<h4>密码重置失败，请稍后重试</h4>");
        } 
        #endregion
    }
}
