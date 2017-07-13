using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Models.ViewModels
{
    public class ProductPageViewModel
    {
        public ProductPageViewModel()
        {
            this.MainCategories = new SelectList(new List<SelectListItem>());
            this.ShoppingCart = new List<OrderItem>();
        }

        public SelectList MainCategories { get; set; }
        public List<OrderItem> ShoppingCart { get; set; }
    }
}