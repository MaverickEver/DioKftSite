using System.ComponentModel.DataAnnotations;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }        
    }
}
