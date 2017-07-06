using DioKftSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace DioKftSite.Helpers
{
    public class Importer : IDisposable
    {
        private const char COLUMN_DELIMETER = '\t';
        private const char LINE_END = '\n';
        private readonly string[] columns = new string[] { "Név","Kiszerelés","Főkategória","Alkategória","Származási hely","Gyártó","Minőség","Típus","Kultúra","Felhasználási terület"};
        private DioKftEntities context;

        public Importer()
        {
            this.context = new DioKftEntities();
        }

        public void ClearDatabase()
        {
            using (var context = new DioKftEntities())
            {                
                context.Categories.RemoveRange(context.Categories);                
                context.Units.RemoveRange(context.Units);
                context.Products.RemoveRange(context.Products);

                context.SaveChanges();
            }

            Console.WriteLine("Database Cleared.");
        }

        public string ExecuteImportLogic(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);
            var messageQue = new StringBuilder();
            var rows = fileContent.Split(LINE_END);

            if (!this.ValidateColumns(rows.FirstOrDefault(), messageQue)) return messageQue.ToString();

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    InitializeDatabase(rows.Skip(1), messageQue);

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return messageQue.ToString();
        }

        private void InitializeDatabase(IEnumerable<string> rows, StringBuilder errorMessage)
        {
            foreach (var row in rows)
            {
                var cells = row.Split(COLUMN_DELIMETER);

                if (cells.Length != 10)
                {
                    errorMessage.AppendLine($"{string.Join("|", cells)}");
                    continue;
                }

                var product = new Product
                {
                    Name = cells[0],
                    UnitId = this.CreateUnit(cells[1]),
                    CategoryId = this.CreateCategory(cells[2], cells[3]),
                    PlaceOfOrigin = cells[4],
                    Manufacturer = cells[5],                    
                    Quality = cells[6],
                    Type = cells[7],
                    Culture = cells[8],
                    AreaOfUsage = cells[9]
                };

                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        private bool ValidateColumns(string row, StringBuilder messageQue)
        {
            var columns = row.Split(COLUMN_DELIMETER);

            for (var i = 0; i < columns.Length; i++)
            {
                if (columns[i] != this.columns[i])
                {
                    messageQue.AppendLine($"Invalid column: original->{this.columns[i]} | imported->{columns[i] ?? "unknown"}");
                }
            }

            return messageQue.Length == 0;
        }

        private int CreateCategory(string mainCategory, string subCategory)
        {
            var parentId = this.CreateCategory(mainCategory);

            if (parentId == 0)
            {
                throw new InvalidOperationException("MainCategoriy cannot be created.");
            }

            if (string.IsNullOrEmpty(subCategory)) { return parentId; }

            return this.CreateCategory(subCategory, parentId);
        }

        private int CreateCategory(string name, int? parentId = null)
        {
            int id = (from i in this.context.Categories
                      where i.Name == name
                      select i.Id).FirstOrDefault();

            if (id != 0) return id;

            var item = new Category { Name = name, ParentId = parentId };
            this.context.Categories.Add(item);
            context.SaveChanges();

            return item.Id;
        }

        private int CreateUnit(string name)
        {
            int id = (from i in this.context.Units
                      where i.Name == name
                      select i.Id).FirstOrDefault();

            if (id != 0) return id;

            var item = new Unit { Name = name };
            this.context.Units.Add(item);
            context.SaveChanges();

            return item.Id;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                    this.context = null;
                }
            }
        }

    }
}
