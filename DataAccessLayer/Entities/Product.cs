using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class Product : EntityBase
    {
       
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public int UnitId { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("UnitId")]
        public Unit Unit { get; set; }

        [InverseProperty("Product")]
        public List<ProductDocument> ProductDocuments { get; set; }

    }
}