using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DioKftSite.Models;
using DioKftSite.Helpers;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class NewsCategoriesController : Controller
    {
        private DioKftEntities db = new DioKftEntities();

        // GET: NewsCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.NewsCategories.ToListAsync());
        }

        // GET: NewsCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newsCategory = await db.NewsCategories.FindAsync(id);
            if (newsCategory == null)
            {
                return HttpNotFound();
            }
            return View(newsCategory);
        }

        // GET: NewsCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] NewsCategory newsCategory)
        {
            if (ModelState.IsValid)
            {
                db.NewsCategories.Add(newsCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newsCategory);
        }

        // GET: NewsCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newsCategory = await db.NewsCategories.FindAsync(id);
            if (newsCategory == null)
            {
                return HttpNotFound();
            }
            return View(newsCategory);
        }

        // POST: NewsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] NewsCategory newsCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsCategory);
        }

        // GET: NewsCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsCategory newsCategory = await db.NewsCategories.FindAsync(id);
            if (newsCategory == null)
            {
                return HttpNotFound();
            }
            return View(newsCategory);
        }

        // POST: NewsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NewsCategory newsCategory = await db.NewsCategories.FindAsync(id);
            db.NewsCategories.Remove(newsCategory);
            await db.SaveChangesAsync();
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
    }
}
