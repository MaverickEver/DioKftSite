using DioKftSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class NewsPageController : Controller
    {
        // GET: NewsPage
        public ActionResult Index()
        {
            var news = new List<News>();
            using (var db = new DioKftEntities())
            {
                news = db.News.ToList();
            }

            return View(news);
        }

        // GET: News/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News news;
            using (var db = new DioKftEntities())
            {
                news = await db.News.FindAsync(id);
            }

            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }
    }
}