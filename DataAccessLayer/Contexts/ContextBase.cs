using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Contexts
{
    public class ContextBase : DbContext
    {
        public ContextBase() : base("DioKftDatabaseConnection")
        {

        }
    }
}
