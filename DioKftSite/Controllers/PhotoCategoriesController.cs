using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DioKftSite.Models;
using DioKftSite.Helpers;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class PhotoCategoriesController : Controller
    {
        private DioKftEntities db = new DioKftEntities();

        // GET: PhotoCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.PhotoCategories.ToListAsync());
        }

        // GET: PhotoCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = await db.PhotoCategories.FindAsync(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            return View(photoCategory);
        }

        // GET: PhotoCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] PhotoCategory photoCategory)
        {
            if (ModelState.IsValid)
            {
                db.PhotoCategories.Add(photoCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(photoCategory);
        }

        // GET: PhotoCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = await db.PhotoCategories.FindAsync(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            return View(photoCategory);
        }

        // POST: PhotoCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] PhotoCategory photoCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(photoCategory);
        }

        // GET: PhotoCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoCategory photoCategory = await db.PhotoCategories.FindAsync(id);
            if (photoCategory == null)
            {
                return HttpNotFound();
            }
            return View(photoCategory);
        }

        // POST: PhotoCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhotoCategory photoCategory = await db.PhotoCategories.FindAsync(id);
            db.PhotoCategories.Remove(photoCategory);
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
