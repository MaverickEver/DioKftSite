using MS.WebSolutions.DioKft.DataAccessLayer.Contexts;
using MS.WebSolutions.DioKft.DataAccessLayer.Entities;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace MS.WebSolutions.DioKft.ProductImporter
{
    public class Importer
    {
        public void ClearDatabase()
        {
            using (var context = new ProductContext())
            {
                context.ProductDocuments.RemoveRange(context.ProductDocuments);
                context.Categories.RemoveRange(context.Categories);
                context.Manufacturers.RemoveRange(context.Manufacturers);
                context.Units.RemoveRange(context.Units);
                context.Products.RemoveRange(context.Products);

                context.SaveChanges();
            }

            Console.WriteLine("Database Cleared.");
        }

        public string ExecuteImportLogic(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);

            var rows = fileContent.Split('\n').Skip(1);
            var missedRows = new StringBuilder();

            using (var context = new ProductContext())
            {
                foreach (var row in rows)
                {
                    var cells = row.Split('|');

                    if (cells.Length != 4)
                    {
                        missedRows.AppendLine($"{string.Join("|", cells)}");
                        continue;
                    }

                    var product = new Product
                    {
                        Name = cells[1],
                        ManufacturerId = this.CreateEntity(cells[0], context, context.Manufacturers),
                        CategoryId = this.CreateEntity(cells[2], context, context.Categories),
                        UnitId = this.CreateEntity(cells[3], context, context.Units)
                    };

                    context.Products.Add(product);
                    context.SaveChanges();
                }
            }

            return missedRows.ToString();
        }

        private int CreateEntity<TEntity>(string name, DbContext context, DbSet<TEntity> list) where TEntity : EntityBase, new()
        {
            int id = (from i in list
                      where i.Name == name
                      select i.Id).FirstOrDefault();

            if (id != 0) return id;

            var item = new TEntity { Name = name };
            list.Add(item);
            context.SaveChanges();

            return item.Id;
        }
    }
}
