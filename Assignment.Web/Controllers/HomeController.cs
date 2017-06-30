using System.Web.Mvc;

namespace Assignment.Web.Controllers.V1
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}
