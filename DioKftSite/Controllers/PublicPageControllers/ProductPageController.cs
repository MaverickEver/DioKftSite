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
            //var list = new List<Product>();
            //using (var db = new DioKftEntities())
            //{
            //    var query  = db.Products.Include("Unit");
            //    list = (from p in query
            //            select p).ToList();
            //}

            return View();
        }
    }
}