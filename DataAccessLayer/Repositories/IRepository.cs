using System.Collections.Generic;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Repositories
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        bool TryGet(TKey id, out TEntity entity);
        void Save(TEntity entity);
        void Delete(TKey id);
        IEnumerable<TEntity> ListAll();
    }
}
