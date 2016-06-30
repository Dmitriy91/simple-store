using System.Web.Mvc;

namespace Assignment.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}
