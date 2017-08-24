using DioKftSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DioKftSite.Models;
using System.Net.Mail;
using DioSiteKft.Models;
using System.Text;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class ContactPageController : Controller
    {
        // GET: ContactPage
        public ActionResult Index()
        {
            var viewModel = BuildViewModel();
            return View(viewModel);
        }

        private ContactViewModel BuildViewModel()
        {
            var viewModel = new ContactViewModel { Contacts = this.GetAllContacts() };

            var shoppingCart = (this.Session[ProductPageController.SHOPPING_CART] as Dictionary<string, OrderItem>)?.Values?.ToList() ?? new List<OrderItem>();
            if ((shoppingCart?.Count ?? 0) > 0)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Ajánlatkérés a következő tétel(ek)re: ");

                foreach (var item in shoppingCart)
                {
                    stringBuilder.AppendLine($"Termék: {item.ProductName}, {item.Quantity} X {item.UnitName}");
                }

                viewModel.Email.Message = stringBuilder.ToString();
            }

            return viewModel;
        }

        [HttpPost]
        public ActionResult SendEmail(Email email)
        {
            try
            {
                var message = CreateEmailMessage(email);

                this.SendEmailMessage(message);
                
            }
            catch (Exception ex)
            {
                return Json("A problem has occured during contacting us. Please try it again! Exception: " + ex.Message);
            }

            ViewBag.Message = "Az email sikeresen elküldve.";
            var viewModel = BuildViewModel();
            return View("Index", viewModel);
        }

        private IEnumerable<Contact> GetAllContacts()
        {
            var contactList = new List<Contact>();

            try
            {
                using (var db = new DioKftEntities())
                {
                    contactList = (from p in db.Contacts
                                   select p).ToList();
                }
            }
            catch
            {
                //TODO: Error handling
                return contactList;
            }

            return contactList;
        }

        private void SendEmailMessage(SendGridMessage message)
        {
#if Debug
            var apiKey = "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";
#else
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
#endif

            var client = new SendGridClient(apiKey);
            client.SendEmailAsync(message).Wait();                                 
        }

        private string FetchSettingsValueFromDatabase(string name)
        {
            using (var databaseContext = new DioKftEntities())
            {
               return databaseContext.WebSiteConfigurations.Where(c => c.Name == name).FirstOrDefault()?.Value;
            }
        }

        private SendGridMessage CreateEmailMessage(Email newEmail)
        {
            var from = new EmailAddress(newEmail.EmailAddress);
            var to = new EmailAddress(FetchSettingsValueFromDatabase("Email"));
            var htmlContent = $"<p>Email From: {newEmail.Name} ({newEmail.EmailAddress})</p><p>Message:</p><p>{newEmail.Message}</p>";

            var message = MailHelper.CreateSingleEmail(from, to, newEmail.Subject, string.Empty, htmlContent);
            return message;
        }
    }
}