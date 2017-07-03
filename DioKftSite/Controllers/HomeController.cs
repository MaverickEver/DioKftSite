using DioKftSite.Helpers;
using System.Web.Mvc;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}