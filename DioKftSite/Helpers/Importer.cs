using DioKftSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task ClearDatabaseAsync()
        {
            using (var context = new DioKftEntities())
            {
                context.Categories.RemoveRange(context.Categories);
                context.Units.RemoveRange(context.Units);
                context.Products.RemoveRange(context.Products);

                await context.SaveChangesAsync();
            }

            Console.WriteLine("Database Cleared.");
        }

        public async Task<string> ExecuteImportLogicAsync(Stream inputStream, bool forceClear)
        {
            if (forceClear) { await this.ClearDatabaseAsync(); }            

            var fileContent = new StreamReader(inputStream, Encoding.UTF8, true).ReadToEnd();

            var messageQue = new StringBuilder();
            var rows = fileContent.Split(LINE_END);

            if (!this.ValidateColumns(rows.FirstOrDefault(), messageQue)) return messageQue.ToString();

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    await InitializeDatabaseAsync(rows.Skip(1), messageQue);

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return messageQue.ToString().Trim();
        }

        private async Task InitializeDatabaseAsync(IEnumerable<string> rows, StringBuilder errorMessage)
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
                    Name = cells[0].Trim(),
                    UnitId = await this.CreateUnitAsync(cells[1].Trim()),
                    CategoryId = await this.CreateCategoryAsync(cells[2].Trim(), cells[3].Trim()),
                    PlaceOfOrigin = cells[4].Trim(),
                    Manufacturer = cells[5].Trim(),                    
                    Quality = cells[6].Trim(),
                    Type = cells[7].Trim(),
                    Culture = cells[8].Trim(),
                    AreaOfUsage = cells[9].Trim()
                };

                context.Products.Add(product);
                await context.SaveChangesAsync();
            }
        }

        private bool ValidateColumns(string row, StringBuilder messageQue)
        {
            var columns = row.Split(COLUMN_DELIMETER);

            for (var i = 0; i < columns.Length; i++)
            {
                if (columns[i].Trim() != this.columns[i])
                {
                    messageQue.AppendLine($"Invalid column: original->{this.columns[i]} | imported->{columns[i] ?? "unknown"}");
                }
            }

            return messageQue.Length == 0;
        }

        private async Task<int> CreateCategoryAsync(string mainCategory, string subCategory)
        {
            var parentId = await this.CreateCategoryAsync(mainCategory);

            if (parentId == 0)
            {
                throw new InvalidOperationException("MainCategoriy cannot be created.");
            }

            if (string.IsNullOrEmpty(subCategory)) { return parentId; }

            return  await this.CreateCategoryAsync(subCategory, parentId);
        }

        private async Task<int> CreateCategoryAsync(string name, int? parentId = null)
        {
            int id = (from i in this.context.Categories
                      where i.Name == name
                      select i.Id).FirstOrDefault();

            if (id != 0) return id;

            var item = new Category { Name = name, ParentId = parentId };
            this.context.Categories.Add(item);
            await context.SaveChangesAsync();

            return item.Id;
        }

        private async Task<int> CreateUnitAsync(string name)
        {
            int id = (from i in this.context.Units
                      where i.Name == name
                      select i.Id).FirstOrDefault();

            if (id != 0) return id;

            var item = new Unit { Name = name };
            this.context.Units.Add(item);
            await context.SaveChangesAsync();

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
