using System.ComponentModel.DataAnnotations.Schema;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class Image : EntityBase
    {

        [Column(TypeName = "image")]
        public byte[] BinaryData;

        public int? ImageCategoryId { get; set; }               

        [ForeignKey("ImageCategoryId")]
        public Category ImageCategory { get; set; }

    }
}
