using DioKftSite.Models;
using DioKftSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class ProductPageController : Controller
    {
        public const string SHOPPING_CART = "ShoppingCart";

        // GET: ProductPage
        public ActionResult Index()
        {
            SelectList categories;
            using (var db = new DioKftEntities())
            {
                categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            var model = new ProductPageViewModel {
                MainCategories = categories,
                ShoppingCart = (this.Session[SHOPPING_CART] as Dictionary<string, OrderItem>)?.Values?.ToList() ?? new List<OrderItem>()
            };

            return View(model);
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

        public ActionResult AddProdutToCart(int productId, int quantity)
        {            
            using (var db = new DioKftEntities())
            {
                var product = db.Products.Find(productId);                
            
                var shoppingCart = this.Session[SHOPPING_CART] as Dictionary<string, OrderItem> ?? new Dictionary<string, OrderItem>();

                if (shoppingCart.ContainsKey(productId.ToString()))
                {
                    (shoppingCart[productId.ToString()] as OrderItem).Quantity += quantity;
                }
                else
                {
                    shoppingCart.Add(productId.ToString(), new OrderItem { ProductId = product.Id, ProductName = product.Name, Quantity = quantity, UnitName = product?.Unit?.Name});
                }

                this.Session[SHOPPING_CART] = shoppingCart;
                return Json(shoppingCart.Values, JsonRequestBehavior.AllowGet);
            }            
        }

        public ActionResult RemoveProdutFromCart(int productId)
        {
            var shoppingCart = this.Session[SHOPPING_CART] as Dictionary<string, OrderItem> ?? new Dictionary<string, OrderItem>();

            if (shoppingCart.ContainsKey(productId.ToString()))
            {
                shoppingCart.Remove(productId.ToString());
            }            

            return Json(shoppingCart.Values, JsonRequestBehavior.AllowGet);
        }
    }
}