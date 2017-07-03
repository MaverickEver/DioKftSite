using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Repositories
{
    public interface IAsyncRepository<TEntity, in TKey> where TEntity : class
    {
        Task<bool> DeleteAsync(TKey[] ids);
        Task<bool> SaveAsync(TEntity entity);
        Task<IList<TEntity>> TryGetAsync(TKey[] ids, int bunchSize = 5);
    }
}