using MS.WebSolutions.DioKft.DataAccessLayer.Entities;
using System.Data.Entity;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Contexts
{
    public class ContactContext : ContextBase
    {
        public DbSet<Contact> Contacts { get; set; }
    }
}
