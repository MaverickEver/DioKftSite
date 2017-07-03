using IO = System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace DioKftSite.Controllers
{
    public class ImageUploaderController : Controller
    {
        // GET: ImageUploader
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> UploadAsync()
        {
            if (!Request.ContentType.Contains("multipart/form-data") || (Request?.Files?.Count ?? 0) != 1 ) { return "File uploading has been failed!"; }
           
            var extension = IO.Path.GetExtension(Request.Files[0].FileName);
            var stream = Request.Files[0].InputStream;

            var result = await this.SaveAsync(stream, extension, FileLocations.ContactImages);

            return string.IsNullOrEmpty(result) ? "Failed" : result;
        }

        private async Task<string> SaveAsync(IO.Stream inputStream, string extension, FileLocations location)
        {
            var path = string.Empty;
            try
            {
                var folderPath = Server.MapPath($"~/Content/DynamicResources/{location.ToString()}");

                if (!IO.Directory.Exists(folderPath))
                {
                    IO.Directory.CreateDirectory(folderPath);
                }

                path = $"~/Content/DynamicResources/{location.ToString()}/{Guid.NewGuid()}{extension}";
                var serverPath = Server.MapPath(path);
                                
                using (var fileStream = IO.File.Create(serverPath))
                {
                    await inputStream.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }

            return path;
        }
    }

    public enum FileLocations
    {
        ContactImages,
        Gallery,
        NewsImages,
        ImportFolder
    }
}