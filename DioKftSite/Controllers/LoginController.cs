using DioKftSite.Helpers;
using DioKftSite.Models;
using System.Linq;
using System.Web.Mvc;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "Name, Password")] UserCredentials credentials)
        {
            var encryptedPassword = Encryptor.EncrypteString(credentials.Password);

            User user = null;
            using (var db = new DioKftEntities())
            {
                user = db.Users.SingleOrDefault(u => u.Name == credentials.Name);
            }

            if (!string.IsNullOrEmpty(user?.Password) && user.Password == encryptedPassword)
            {
                Session[SessionItems.UserName.ToString()] = user.Name;
                Session[SessionItems.UserId.ToString()] = user.Id.ToString();                
            }
            else
            {
                ViewBag.IsUnauthorized = true;
                return View("Index");
            }

            return RedirectToAction("Admin", "Home");
        }

        public ActionResult Logout()
        {
            Session.Remove(SessionItems.UserName.ToString());
            Session.Remove(SessionItems.UserId.ToString());
            
            return RedirectToAction("Index", "Home");
        }
    }
}