using MS.WebSolutions.DioKft.DataAccessLayer.Entities;
using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Contexts
{
    public class ImageContext : ContextBase
    {
        public DbSet<Image> Images{ get; set; }
        public DbSet<ImageCategory> ImageCategories { get; set; }
    }
}
