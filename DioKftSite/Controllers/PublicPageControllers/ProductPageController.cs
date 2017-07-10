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

            return View(categories);
        }

        public ActionResult GetSubCategories(int mainCategoryId)
        {
            SelectList categories;
            using (var db = new DioKftEntities())
            {
                categories = new SelectList(db.Categories.Where(c =>c.ParentId == mainCategoryId).ToList(), "Id", "Name");
            }

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(int mainCategoryId, int subCategoryId)
        {
            
            using (var db = new DioKftEntities())
            {
                var products = db.Products
                            .Include("Unit")
                            .Where(p => subCategoryId <= 0 ? p.CategoryId == mainCategoryId : p.CategoryId == subCategoryId)
                            .OrderBy(p => p.Name).ThenBy(p => p.Unit.Name)
                            .Select(p => new { p.Id, p.Name, UnitName = p.Unit.Name, p.Manufacturer, p.PlaceOfOrigin, p.Quality, p.Type, p.Culture, p.AreaOfUsage}).ToList();

                return Json(products, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}