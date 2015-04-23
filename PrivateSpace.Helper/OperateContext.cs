using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PrivateSpace.Helper
{
    public class OperateContext
    {
        private static OperateContext current = null;
        private static Common.LogHelper logger = new Common.LogHelper(typeof(OperateContext));

        private HttpServerUtility Server { get { return HttpContext.Current.Server; } }

        private OperateContext()
        {
            BLLSession = new BLLSessionFactory().GetBLLSession();
            //Spring.Net 获取
            //BLLSession = DI.SpringHelper.GetObject<IBLL.IBLLSession>("BLLSession");
        }

        public IBLL.IBLLSession BLLSession;

        public static OperateContext Current
        {
            get
            {
                if (current == null)
                {
                    current = CallContext.GetData(typeof(OperateContext).Name) as OperateContext;
                    if (current == null)
                    {
                        current = new OperateContext();
                        CallContext.SetData(typeof(OperateContext).Name, current);
                    }
                }
                return current;
            }
        }

        public static T RequestParams<T>()
        {
            string postString = null;
            //从请求的数据流中获取请求信息
            using (System.IO.Stream stream = HttpContext.Current.Request.InputStream)
            {
                //创建 byte数组以接受从流中获取到的消息
                byte[] postBytes = new byte[stream.Length];
                //将POST请求中的数据流读入 准备好的 byte数组中
                stream.Read(postBytes, 0, (int)stream.Length);
                //从数据流中获取到字符串
                postString = System.Text.Encoding.UTF8.GetString(postBytes);
                //logger.Debug("接收到的POST字符串："+postString);
            }
            return Common.Helper.JsonToObject<T>(postString);
        }

        #region 用户相关 Member
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.LoginState Login(Model.ViewModel.LoginViewModel model)
        {
            //抽象工厂实现方式
            //BLLFactory.BLLAbsFactory fac = BLLFactory.BLLAbsFactory.GetBLLFactory();
            //IBLL.IUser userBLL = fac.GetUser();

            //依赖注入
            //IBLL.IUser userBLL = Current.BLLSession.IUser;

            //判断浏览器端是否有cookie   ----- 在请求页面时处理
            Model.User u = SelectMember(model.Email);
            if (u.AuthCode != null)
            {
                return Model.LoginState.RequireVerification;
            }
            //判断 model.RememberMe 的状态
            Model.LoginState state = BLLSession.IUser.Login(model);
            //判断是否登录成功
            if (state == Model.LoginState.Success)
            {
                //登录成功，设置Session
                HttpContext.Current.Session["Member"] = SelectMember(model.Email);
                //登录成功并且选中 “记住我” 复选框，则将用户名、密码（需要加密）保存到cookie
                if (model.RememberMe)
                {
                    //创建cookie
                    HttpCookie cookieUid = new HttpCookie("uid", Common.Helper.EncryptUserInfo(model.Email));
                    HttpCookie cookiePwd = new HttpCookie("pwd", Common.Helper.EncryptUserInfo(model.Password));
                    //设置过期时间
                    cookieUid.Expires = DateTime.Now.AddDays(1);
                    cookiePwd.Expires = DateTime.Now.AddDays(1);
                    //将cookie输出到浏览器
                    HttpContext.Current.Response.Cookies.Add(cookieUid);
                    HttpContext.Current.Response.Cookies.Add(cookiePwd);
                }
            }
            //返回登录状态
            return state;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Register(Model.ViewModel.RegisterViewModel model)
        {
            if (BLLSession.IUser.Regist(model))
            {
                //提示激活账户，或添加Session 以登录
                Model.User u = SelectMember(model.Email);
                //添加 Session
                //HttpContext.Current.Session["Member"] = u;
                if (SendRegistMail(u))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断Email是否重复
        /// </summary>
        /// <param name="mail">Email</param>
        /// <returns></returns>
        public bool ExistMail(string mail)
        {
            return Current.BLLSession.IUser.Exist(m => m.Mail.ToLower() == mail.ToLower());
        }

        /// <summary>
        /// 根据mail查询用户
        /// </summary>
        /// <param name="mail">Mail</param>
        /// <returns>用户实体</returns>
        public Model.User SelectMember(string mail)
        {
            return BLLSession.IUser.GetListBy(m => m.Mail == mail).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// 发送邮件 （现在是账号激活邮件，可扩展）
        /// </summary>
        /// <param name="m">用户实体</param>
        /// <returns>是否发送邮件成功</returns>
        private bool SendRegistMail(Model.User m)
        {
            try
            {
                //激活链接
                //string link = "http://localhost:35251/Member/ValidAuthCode/?Mail=" + Server.UrlEncode(m.Mail) + "&AuthCode=" + m.AuthCode;
                //
                string link = "http://ftp216703.host500.zhujiwu.cn/Member/ValidAuthCode/?Mail=" + Server.UrlEncode(m.Mail) + "&AuthCode=" + m.AuthCode;
                string template = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/RegistMailTemplate.html"));
                string body = template.Replace("{@Link}", link).Replace("{@Username}", m.Username).Replace("{@RegistTime}", m.RegTime.ToString("yyyy-MM-dd HH:mm:ss")).Replace("{@Today}", DateTime.Today.ToString("yyyy-MM-dd")).Replace("{@WebSite}", "http://privatespace.cn/");
                //创建 MailMessage
                MailAddress mailFrom = new MailAddress("PrivateSpace@aliyun.com", "PrivateSpace网站管理员", System.Text.Encoding.UTF8);
                MailAddress mailTo = new MailAddress(m.Mail, m.Username, System.Text.Encoding.UTF8);
                MailMessage mailMsg = new MailMessage(mailFrom, mailTo);
                mailMsg.IsBodyHtml = true;
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Subject = "PrivateSpace 注册账户激活";
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = body;
                //smtp 服务器
                SmtpClient smtp = new SmtpClient("smtp.aliyun.com");
                //发送邮件凭证
                smtp.Credentials = new System.Net.NetworkCredential("PrivateSpace@aliyun.com", "PrivateSpace123");
                //发送邮件
                smtp.Send(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(OperateContext)).Error(ex.Message + ex.StackTrace);
                return false;
            }
        }

        public bool SendResetPwdMail(Model.User m)
        {
            try
            {
                //激活链接  
                //http://localhost:35251/
                //string link = "http://localhost:35251/Member/ResetPassword/?Email=" + Server.UrlEncode(m.Mail) + "&ValidCode=" + m.AuthCode;
                string link = "http://ftp216703.host500.zhujiwu.cn/Member/ResetPassword/?Email=" + Server.UrlEncode(m.Mail) + "&ValidCode=" + m.AuthCode;
                string template = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/ResetPwdMailTemplate.html"));
                string body = template.Replace("{@Link}", link).Replace("{@Username}", m.Username).Replace("{@Time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Replace("{@Today}", DateTime.Today.ToString("yyyy-MM-dd"));
                //创建 MailMessage
                MailAddress mailFrom = new MailAddress("PrivateSpace@aliyun.com", "PrivateSpace网站管理员", System.Text.Encoding.UTF8);
                MailAddress mailTo = new MailAddress(m.Mail, m.Username, System.Text.Encoding.UTF8);
                MailMessage mailMsg = new MailMessage(mailFrom, mailTo);
                mailMsg.IsBodyHtml = true;
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Subject = "PrivateSpace 密码重置";
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = body;
                //smtp 服务器
                SmtpClient smtp = new SmtpClient("smtp.aliyun.com");
                //发送邮件凭证
                smtp.Credentials = new System.Net.NetworkCredential("PrivateSpace@aliyun.com", "PrivateSpace123");
                //发送邮件
                smtp.Send(mailMsg);
                return true;
            }
            catch (Exception ex)
            {
                new Common.LogHelper(typeof(OperateContext)).Error(ex.Message + ex.StackTrace);
                return false;
            }
        }


        public bool ResetPwd(Model.ViewModel.ResetPasswordViewModel model)
        {
            try
            {
                Model.User m = SelectMember(model.Email);
                if (m.AuthCode == model.Code)
                {
                    m.Password = Common.Helper.GetMD5(model.Password);
                    m.AuthCode = null;
                    if (UpdateUserInfo(m, "Password", "AuthCode"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
                return false;
            }
        }

        public bool ActivateAccount(Model.User u)
        {
            return BLLSession.IUser.ActivateAccount(u);
        }

        public bool UpdateUserInfo(Model.User u,params string[] pros)
        {
            return BLLSession.IUser.Modify(u, pros)>0?true :false;
        }

        public Model.ViewModel.MoodViewModel SelectMood(int moodId)
        {
            return BLLSession.IMood.GetListBy(m => m.moodId == moodId).FirstOrDefault().ToViewModel();
        }

        public Model.ViewModel.DiaryViewModel SelectDiary(int diaryId)
        {
            return BLLSession.IDiary.GetListBy(m => m.diaryId == diaryId).FirstOrDefault().ToViewModel();
        }

        /// <summary>
        /// 添加心情
        /// </summary>
        /// <param name="mood">心情</param>
        /// <param name="u">用户</param>
        /// <returns></returns>
        public Model.ViewModel.MoodViewModel AddMood(Model.ViewModel.MoodViewModel mood, Model.User u)
        {
            Model.Mood m = new Model.Mood() { MoodContent = mood.MoodContent, MoodTime = DateTime.Now, userId = u.userId };
            BLLSession.IMood.Add(m);
            return m.ToViewModel();
        }

        /// <summary>
        /// 添加日记
        /// </summary>
        /// <param name="d">日记</param>
        /// <param name="u">用户</param>
        /// <returns></returns>
        public Model.ViewModel.DiaryViewModel AddDiary(Model.ViewModel.DiaryViewModel d, Model.User u)
        {
            Model.Diary diary = new Model.Diary() { diaryContent = d.DiaryContent, diaryTime = DateTime.Now, userId = u.userId, diaryTitle = d.DiaryTitle };
            BLLSession.IDiary.Add(diary);
            return diary.ToViewModel();
        }


        public bool DelMood(int id)
        {
            try
            {
                BLLSession.IMood.Del(new Model.Mood() { moodId = id });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool DelDiary(int id)
        {
            try
            {
                BLLSession.IDiary.Del(new Model.Diary() { diaryId = id });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateDiary(Model.ViewModel.DiaryViewModel diary)
        {
            Model.Diary d = new Model.Diary() { diaryId=diary.DiaryId,diaryContent = diary.DiaryContent, diaryTitle = diary.DiaryTitle, diaryTime = DateTime.Now };
            if (BLLSession.IDiary.Modify(d, "diaryTitle", "diaryContent") > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        /// <summary>
        /// 加载心情列表
        /// </summary>
        /// <param name="pageIndex">页码索引</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件lambda表达式</param>
        /// <param name="orderBy">排序lambda表达式</param>
        /// <returns>心情列表</returns>
        public List<Model.ViewModel.MoodViewModel> LoadMood<TKey>(int pageIndex, int pageSize, Expression<Func<Model.Mood, bool>> whereLambda, Expression<Func<Model.Mood, TKey>> orderBy,bool isAsc)
        {
            List<Model.ViewModel.MoodViewModel> MoodList = BLLSession.IMood.GetPagedList(pageIndex,pageSize,whereLambda,orderBy,isAsc).Select(m=>m.ToViewModel()).ToList();
            return MoodList;
        }
        
        /// <summary>
        /// 加载日记列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Model.ViewModel.DiaryViewModel> LoadDiary<TKey>(int pageIndex, int pageSize, Expression<Func<Model.Diary, bool>> whereLambda, Expression<Func<Model.Diary, TKey>> orderBy,bool isAsc)
        {
            List<Model.ViewModel.DiaryViewModel> MoodList = BLLSession.IDiary.GetPagedList(pageIndex,pageSize,whereLambda,orderBy,isAsc).Select(m=>m.ToViewModel()).ToList();
            return MoodList;
        }

        public int QueryMoodCount()
        {
            return BLLSession.IMood.GetList().Count;
        }

        public int QueryDiaryCount()
        {
            return BLLSession.IDiary.GetList().Count;
        }
    }
}
