using System;
using System.Linq;
using System.Collections.Generic;
using MS.WebSolutions.DioKft.DataAccessLayer.Contexts;
using MS.WebSolutions.DioKft.DataAccessLayer.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Repositories
{
    public class ImageRepository : RepositoryBase<ImageContext>, IAsyncRepository<Image, int>
    {
        public async Task<bool> DeleteAsync(int[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException($"{nameof(ids)}");
            }

            foreach (var id in ids) {
                Image imageToDelete = new Image { Id = id};
                this.context.Entry(imageToDelete).State = EntityState.Deleted;               
            }

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }        

        public async Task<bool> SaveAsync(Image entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException($"{nameof(entity)}");
            }

            this.context.Images.Add(entity);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IList<Image>> TryGetAsync(int[] ids, int bunchSize = 5)
        {
            var query = this.context.Images.Include("ImageCategory");
            var images = new List<Image>(ids.Length);

            for (int i = 0; i < ids.Length - 5; i+=5)
            {
                var idsToFetch = ids.Take(bunchSize);
                ids = ids.Skip(bunchSize).ToArray();

                var bunchOfImages = await (from p in query
                                   where idsToFetch.Contains(p.Id)
                                   select p).ToListAsync();

                images.AddRange(bunchOfImages);
            }

            return images;
        }

        public IEnumerable<ImageCategory> ListAllCategories()
        {
            var categories = (from c in this.context.ImageCategories
                              select c).ToList();

            return categories;
        }

        public bool TrySaveCategory(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException($"{nameof(title)}");
            }

            var newCategory = new ImageCategory { Name = title };
            this.context.ImageCategories.Add(newCategory);

            var result = this.context.SaveChanges();

            return result > 0;
        }
    }
}
