using DioSiteKft.Models;
using System.Collections.Generic;

namespace DioKftSite.Models.ViewModels
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            this.Contacts = new List<Contact>();
            this.Email = new Email();
        }

        public IEnumerable<Contact> Contacts { get; set; }
        public Email Email { get; set; }
    }
}