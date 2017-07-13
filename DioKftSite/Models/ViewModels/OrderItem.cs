using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DioKftSite.Models.ViewModels
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
    }
}