using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PrivateSpace
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup 
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //注册全局过滤器
            GlobalFilters.Filters.Add(new Logic.Filters.ErrorHandlerAttribute());

            log4net.Config.XmlConfigurator.Configure();

            //将网站根目录写入文本文件
            //using (System.IO.TextWriter writer = System.IO.File.CreateText(Server.MapPath("./App_Data/WebRootPath.txt")))
            //{
            //    writer.Write(Server.MapPath("./"));
            //}
            //配置只读，不能添加或更新
            //Common.ConfigurationHelper.AddAppSetting("WebRootPath", Server.MapPath("./"));
        }
    }
}