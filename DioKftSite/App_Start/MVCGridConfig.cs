[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DioKftSite.MVCGridConfig), "RegisterGrids")]

namespace DioKftSite
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using DioKftSite.Models;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            
            MVCGridDefinitionTable.Add("Products", new MVCGridBuilder<Product>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("Name")
                        .WithHeaderText("Név")
                        .WithValueExpression(i => i.Name); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("Unit")
                        .WithHeaderText("Kiszerelés")
                        .WithValueExpression(i => i.Unit.Name);
                    cols.Add().WithColumnName("Manufacturer")
                        .WithHeaderText("Gyártó")
                        .WithValueExpression(i => i.Manufacturer);
                    cols.Add().WithColumnName("PlaceOfOrigin")
                        .WithHeaderText("Származási hely")
                        .WithValueExpression(i => i.PlaceOfOrigin);
                    cols.Add().WithColumnName("Quality")
                        .WithHeaderText("Minőség")
                        .WithValueExpression(i => i.Quality);
                    cols.Add().WithColumnName("Type")
                        .WithHeaderText("Típus")
                        .WithValueExpression(i => i.Type);
                    cols.Add().WithColumnName("Culture")
                        .WithHeaderText("Kultúra")
                        .WithValueExpression(i => i.Culture);
                    cols.Add().WithColumnName("AreaOfUsage")
                        .WithHeaderText("Felhasználási terület")
                        .WithValueExpression(i => i.AreaOfUsage);
                    cols.Add().WithColumnName("UrlExample")
                        .WithHeaderText("Rendelés")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("Index", "ContactPage"));
                })
                .WithRetrieveDataMethod((context) =>
                {
                    // Query your data here. Obey Ordering, paging and filtering parameters given in the context.QueryOptions.
                    // Use Entity Framework, a module from your IoC Container, or any other method.
                    // Return QueryResult object containing IEnumerable<YouModelItem>
                    var options = context.QueryOptions;
                    int totalRecords;                    
                    string globalSearch = options.GetAdditionalQueryOptionString("search");
                    string sortColumn = options.GetSortColumnData<string>();

                    string categoryIdString;
                    options.AdditionalQueryOptions.TryGetValue("CaregoryId", out categoryIdString);
                    int categoryId;
                    int.TryParse(categoryIdString, out categoryId);

                    IEnumerable<Product> items = new List<Product>();

                    using (var db = new DioKftEntities())
                    {
                        items = db.Products.Include("Unit").Where(p=>p.CategoryId == categoryId || categoryId == 0).ToList();
                        totalRecords = items.Count();
                    }

                    return new QueryResult<Product>()
                    {
                        Items = items,
                        TotalRecords = totalRecords
                    };

                })
            );
            
        }
    }
}