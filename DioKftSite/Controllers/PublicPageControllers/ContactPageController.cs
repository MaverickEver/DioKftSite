using DioKftSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DioKftSite.Models;
using System.Net.Mail;
using DioSiteKft.Models;
using System.Text;

namespace DioKftSite.Controllers.PublicPageControllers
{
    public class ContactPageController : Controller
    {
        // GET: ContactPage
        public ActionResult Index()
        {
            var viewModel = new ContactViewModel { Contacts = this.GetAllContacts() };

            var shoppingCart = (this.Session[ProductPageController.SHOPPING_CART] as Dictionary<string, OrderItem>)?.Values?.ToList() ?? new List<OrderItem>();
            if ((shoppingCart?.Count ?? 0) > 0)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Megrendelt tételek: ");

                foreach (var item in shoppingCart)
                {
                    stringBuilder.AppendLine($"Termék: {item.ProductName}, {item.Quantity} X {item.UnitName}");
                }

                viewModel.Email.Message = stringBuilder.ToString();
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SendEmail(Email email)
        {
            MailMessage message = CreateEmailMessage(email);

            try
            {
                SendEmailMessage(message);
            }
            catch (Exception ex)
            {
                return Json("A problem has occured during contacting us. Please try it again! Exception: " + ex.Message);
            }

            return Json("Email has been sent.");
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

        private void SendEmailMessage(MailMessage message)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }

        private static MailMessage CreateEmailMessage(Email newEmail)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("sapi.mihaly@gmail.com"));
            message.From = new MailAddress(newEmail.EmailAddress);
            message.Subject = "New Request from diokft.hu website.";
            message.Body = string.Format(body, newEmail.Name, newEmail.EmailAddress, newEmail.Message);
            message.IsBodyHtml = true;
            return message;
        }
    }
}