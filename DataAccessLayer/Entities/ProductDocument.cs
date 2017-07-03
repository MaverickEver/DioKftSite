using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class ProductDocument : EntityBase
    {
        [Required]
        public string FileUrl { get; set; }

        public Product Product { get; set; }
    }
}
