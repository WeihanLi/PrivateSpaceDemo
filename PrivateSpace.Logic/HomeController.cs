using System.Web.Mvc;

namespace PrivateSpace.Logic
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {   
            //HttpContext.Response.Write(Server.MapPath("./Bin/"));
            return View();
        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}