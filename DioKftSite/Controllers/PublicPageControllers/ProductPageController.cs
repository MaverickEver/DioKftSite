using DioKftSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class ProductPageController : Controller
    {
        // GET: ProductPage
        public ActionResult Index()
        {
            SelectList categories;
            using (var db = new DioKftEntities())
            {
                categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            ViewBag.CategoryId = 1;
            ViewBag.SubCategoryId = 0;            

            return View(categories);
        }
    }
}