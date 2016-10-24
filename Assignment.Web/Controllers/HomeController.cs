using System.Web.Mvc;

namespace Assignment.Web.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}
