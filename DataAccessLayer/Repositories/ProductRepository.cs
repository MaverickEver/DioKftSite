using System;
using System.Linq;
using System.Collections.Generic;
using MS.WebSolutions.DioKft.DataAccessLayer.Repositories;
using MS.WebSolutions.DioKft.DataAccessLayer.Contexts;
using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class ProductRepository : RepositoryBase<ProductContext>, IRepository<Product, int>
    {
        public void Delete(int id)
        {
            Product productToDelete;
            if (!TryGet(id, out productToDelete)) { return; }

            this.context.Products.Remove(productToDelete);
            this.context.SaveChanges();
        }
                
        public IEnumerable<Product> ListAll()
        {
            var query = this.context.Products.Include("Unit");
            return (from p in query
                    select p).ToList();
        }

        public void Save(Product entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }

            this.context.Products.Add(entity);
            this.context.SaveChanges();
        }

        public bool TryGet(int id, out Product entity)
        {
            var query = this.context.Products.Include("Unit");
            entity = (from p in query
                      where p.Id == id
                      select p).SingleOrDefault();

            return entity != null;
        }

        public IEnumerable<Product> ListAllByCategory(int id)
        {           
            return (from p in this.context.Products
                    join c in this.context.Categories on p.CategoryId equals c.Id
                    where c.Id == id
                    select p).Include(p=>p.Unit).ToList();
        }

        public IEnumerable<Category> ListAllCategories()
        {
            return (from p in this.context.Categories
                    select p).ToList();
        }
    }
}
