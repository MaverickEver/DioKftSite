using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DioKftSite.Controllers
{
    public class AzureStorageController : Controller
    {
        private const string AZURE_CONNECTION = "AzureStorageConnectionString";                

        protected async Task<List<string>> StoreFilesInRequestAsync(FileLocations location)
        {          
            var paths = new List<string>();
            if ((Request?.Files?.Count ?? 0) == 0) return paths;

            try
            {
                var containerName = location.ToString().ToLower();

                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(AZURE_CONNECTION));
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();
                container.SetPermissions( new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                paths = await StoreFiles(container);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await this.RemoveStoredFilesAsync(paths.ToArray());
            }

            return paths;
        }

        protected async Task<int> RemoveStoredFilesAsync( params string[] BlobUris)
        {
            var counter = 0;
            foreach (var uri in BlobUris)
            {
                try
                {
                    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(AZURE_CONNECTION));
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var blockBlob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(uri));
                    //var blockBlob = new CloudBlockBlob(new Uri(uri));

                    await blockBlob?.DeleteAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Cannot delete: {uri}");
                    continue;
                }

                counter++;
            }

            return counter;
        }

        private async Task<List<string>> StoreFiles(CloudBlobContainer container)
        {
            var paths = new List<string>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                if (string.IsNullOrEmpty(file?.FileName) || (file?.ContentLength ?? 0)== 0) continue;

                var extension = Path.GetExtension(file.FileName);                

                var blob = container.GetBlockBlobReference($"{Guid.NewGuid()}{extension}");

                using (var inputStream = file.InputStream)
                {
                    await blob.UploadFromStreamAsync(inputStream);
                }                

                paths.Add(blob.Uri.AbsoluteUri);
            }            

            return paths;
        }
    }
}