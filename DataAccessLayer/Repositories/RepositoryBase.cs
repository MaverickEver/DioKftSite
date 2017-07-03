using System;
using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Repositories
{
    public abstract class RepositoryBase<TContext> : IDisposable where TContext : DbContext, new()
    {
        protected readonly TContext context;

        public RepositoryBase()
        {
            this.context = new TContext();
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
                if (context != null)
                {
                    context.Dispose();
                }
            }
        }
    }
}
