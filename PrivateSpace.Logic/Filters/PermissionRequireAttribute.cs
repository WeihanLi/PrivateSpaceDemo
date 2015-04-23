using System.Web.Mvc;

namespace PrivateSpace.Logic.Filters
{
    public class PermissionRequireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Member"]==null)
            {
                filterContext.Result = new RedirectResult("~/");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
