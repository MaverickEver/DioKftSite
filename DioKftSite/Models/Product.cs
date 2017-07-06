//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DioKftSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int UnitId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string Quality { get; set; }
        public string Type { get; set; }
        public string Culture { get; set; }
        public string AreaOfUsage { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
