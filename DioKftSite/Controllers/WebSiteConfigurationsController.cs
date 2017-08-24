using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DioKftSite.Models;

namespace DioKftSite.Controllers
{
    public class WebSiteConfigurationsController : Controller
    {
        private DioKftEntities db = new DioKftEntities();

        // GET: WebSiteConfigurations
        public async Task<ActionResult> Index()
        {
            return View(await db.WebSiteConfigurations.ToListAsync());
        }

        // GET: WebSiteConfigurations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebSiteConfiguration webSiteConfiguration = await db.WebSiteConfigurations.FindAsync(id);
            if (webSiteConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(webSiteConfiguration);
        }

        // GET: WebSiteConfigurations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WebSiteConfigurations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value")] WebSiteConfiguration webSiteConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.WebSiteConfigurations.Add(webSiteConfiguration);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(webSiteConfiguration);
        }

        // GET: WebSiteConfigurations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebSiteConfiguration webSiteConfiguration = await db.WebSiteConfigurations.FindAsync(id);
            if (webSiteConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(webSiteConfiguration);
        }

        // POST: WebSiteConfigurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value")] WebSiteConfiguration webSiteConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(webSiteConfiguration).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(webSiteConfiguration);
        }

        // GET: WebSiteConfigurations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebSiteConfiguration webSiteConfiguration = await db.WebSiteConfigurations.FindAsync(id);
            if (webSiteConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(webSiteConfiguration);
        }

        // POST: WebSiteConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WebSiteConfiguration webSiteConfiguration = await db.WebSiteConfigurations.FindAsync(id);
            db.WebSiteConfigurations.Remove(webSiteConfiguration);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
