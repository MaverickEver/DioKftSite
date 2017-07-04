using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using DioKftSite.Models;
using DioKftSite.Helpers;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class PhotosController : AzureStorageController
    {
        private DioKftEntities db = new DioKftEntities();

        // GET: Photos
        public async Task<ActionResult> Index()
        {
            var photos = db.Photos.Include(p => p.PhotoCategory);
            return View(await photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.ImageCategoryId = new SelectList(db.PhotoCategories, "Id", "Name");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ImageCategoryId,ImageUrl")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                var paths = await this.StoreFilesInRequestAsync(FileLocations.Gallery);

                if (paths.Count > 0)
                {
                    foreach (var path in paths)
                    {
                        db.Photos.Add(new Photo { ImageCategoryId = photo.ImageCategoryId, ImageUrl = path, Title = photo.Title });                        
                    }

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.ImageCategoryId = new SelectList(db.PhotoCategories, "Id", "Name", photo.ImageCategoryId);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageCategoryId = new SelectList(db.PhotoCategories, "Id", "Name", photo.ImageCategoryId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ImageCategoryId,ImageUrl")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                var paths = await this.StoreFilesInRequestAsync(FileLocations.Gallery);

                if (paths.Count == 1)
                {
                    await this.RemoveStoredFilesAsync(photo.ImageUrl);
                    photo.ImageUrl = paths[0];
                }

                db.Entry(photo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ImageCategoryId = new SelectList(db.PhotoCategories, "Id", "Name", photo.ImageCategoryId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Photo photo = await db.Photos.FindAsync(id);
            db.Photos.Remove(photo);
            await db.SaveChangesAsync();
            await this.RemoveStoredFilesAsync(photo.ImageUrl);
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
