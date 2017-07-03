using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DioKftSite.Models;
using DioKftSite.Helpers;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class NewsController : FilePersistanceController
    {
        private DioKftEntities db = new DioKftEntities();

        // GET: News
        public async Task<ActionResult> Index()
        {
            var news = db.News.Include(n => n.NewsCategory);
            return View(await news.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Header,Body,ImageUrl,VideoUrl,CategoryId,Created,Modified")] News news)
        {
            if (ModelState.IsValid)
            {
                var paths = await this.SaveFilesInRequestAsync(FileLocations.NewsImages);
                
                news.ImageUrl = paths.Count == 1 ? paths[0] : null;
                news.Created = DateTime.Now;
                news.VideoUrl = this.FormatYoutubeVideoUrl(news.VideoUrl);

                db.News.Add(news);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Header,Body,ImageUrl,VideoUrl,CategoryId,Created,Modified")] News news)
        {
            if (ModelState.IsValid)
            {
                var paths = await this.SaveFilesInRequestAsync(FileLocations.NewsImages);

                if (paths.Count == 1)
                {
                    await this.RemoveSavedFilesFromFileSystemAsync(news.ImageUrl);
                    news.ImageUrl = paths[0];
                }
                
                news.Modified = DateTime.Now;
                news.VideoUrl = this.FormatYoutubeVideoUrl(news.VideoUrl);

                db.Entry(news).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            News news = await db.News.FindAsync(id);
            db.News.Remove(news);
            await db.SaveChangesAsync();
            await this.RemoveSavedFilesFromFileSystemAsync(news.ImageUrl);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string FormatYoutubeVideoUrl(string inputUrl)
        {
            const string WATCH_RPEFIX = "watch?v=";
            const string EMBED_URL = "https://www.youtube.com/embed/";
            var videoId = string.Empty;

            if (string.IsNullOrEmpty(inputUrl)) return null;

            try
            {
                var lastTag = inputUrl.Split('/')?.Last();
                videoId = lastTag?.Replace(WATCH_RPEFIX, string.Empty);
            }
            catch
            {
                //TODO: Logging
                return null;
            }

            return string.IsNullOrEmpty(videoId) ? null : $"{EMBED_URL}{videoId}";
        }
    }
}
