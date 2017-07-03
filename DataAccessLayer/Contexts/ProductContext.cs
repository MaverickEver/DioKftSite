using MS.WebSolutions.DioKft.DataAccessLayer.Entities;
using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Contexts
{
    public class ProductContext : ContextBase
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }
    }
}
