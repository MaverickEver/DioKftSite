using DioKftSite.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DioKftSite.Controllers
{
    [CustomAuthorize]
    public class ProductImportController : Controller
    {
        // GET: ProductImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadCsv(bool forceClean = false)
        {
            var numberofFiles = Request?.Files?.Count ?? 0;
            if (numberofFiles == 0 || numberofFiles > 1)
            {
                ViewBag.Error = "No or too much uploaded file(s)";
                return View();
            }

            using (var importProcess = new Importer())
            {                
                if (Request?.Files[0]?.InputStream == null)
                {
                    ViewBag.Error = "Wrong file content";
                    return View();
                }

                var message = await importProcess.ExecuteImportLogicAsync(Request.Files[0].InputStream, forceClean);

                if (!string.IsNullOrEmpty(message))
                {
                    ViewBag.Error = message;
                    return View();
                }
            }

            return RedirectToAction("Index", "Products");
        }
    }
}