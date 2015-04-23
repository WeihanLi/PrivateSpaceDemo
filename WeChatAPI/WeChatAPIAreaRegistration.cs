using System.Web.Mvc;

namespace PrivateSpace.WeChatAPI
{
    public class WeChatAPIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WeChatAPI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WeChatAPI_default",
                "WeChatAPI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces:new string[] { "PrivateSpace.WeChatAPI" }
            );
        }
    }
}