using DioKftSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class GalleryPageController : Controller
    {
        // GET: GalleryPage
        public ActionResult Index()
        {
            return View(this.GetAllPhotos());
        }

        private IEnumerable<Photo> GetAllPhotos()
        {
            var photos = new List<Photo>();

            try
            {
                using (var db = new DioKftEntities())
                {
                    photos = (from p in db.Photos
                                   select p).ToList();
                }
            }
            catch
            {
                //TODO: Error handling
                return photos;
            }

            return photos;
        }
    }
}