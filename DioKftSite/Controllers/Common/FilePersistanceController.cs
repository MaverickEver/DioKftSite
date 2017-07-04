using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers
{
    public class FilePersistanceController : Controller
    {
        private const string DYNAMIC_RESOURCES = "~/Content/DynamicResources/";

        protected async Task<List<string>> SaveFilesInRequestAsync(FileLocations location)
        {          
            var paths = new List<string>();
            if ((Request?.Files?.Count ?? 0) == 0) return paths;

            try
            {                
                var relativeFolderPath = $"{DYNAMIC_RESOURCES}{location.ToString()}";
                var absoluteFolderPath = Server.MapPath(relativeFolderPath);

                if (!Directory.Exists(absoluteFolderPath))
                {
                    Directory.CreateDirectory(absoluteFolderPath);
                }

                paths = await SaveFiles(relativeFolderPath);
            }
            catch (Exception ex)
            {
                //$"Failed: {ex.Message}";
                Console.WriteLine(ex.Message);
                await this.RemoveSavedFilesFromFileSystemAsync(paths.ToArray());
            }

            return paths;
        }

        protected async Task<int> RemoveSavedFilesFromFileSystemAsync( params string[] relativePaths)
        {
            return await Task.Factory.StartNew(() => { return this.RemoveSavedFilesFromFileSystem(relativePaths); });
        }

        private async Task<List<string>> SaveFiles(string relativeFolderPath)
        {
            var paths = new List<string>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                if (string.IsNullOrEmpty(file?.FileName) || (file?.ContentLength ?? 0)== 0) continue;

                var extension = Path.GetExtension(file.FileName);
                var inputStream = file.InputStream;

                string path = $"{relativeFolderPath}/{Guid.NewGuid()}{extension}";
                var serverPath = Server.MapPath(path);

                using (var fileStream = System.IO.File.Create(serverPath))
                {
                    await inputStream.CopyToAsync(fileStream);
                }

                paths.Add(path);
            }            

            return paths;
        }

        private int RemoveSavedFilesFromFileSystem(params string[] relativePaths)
        {
            var counter = 0;
            foreach (var relativefilePath in relativePaths)
            {
                var absolutePath = Server.MapPath(relativefilePath);

                try
                {                    
                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                }
                catch
                {
                    Console.WriteLine($"Cannot delete: {absolutePath}");
                    continue;                    
                }

                counter++;
            }

            return counter;
        }
    }
}