using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.Mobile.Server.Config;
using XamarinChallengeDemoService.Models;

namespace XamarinChallengeDemoService.Controllers
{
    [Authorize]
    [MobileAppController]
    public class GetStorageTokenController : ApiController
    {
        private const string connString = "MS_AzureStorageAccountConnectionString";
        private const string containerName = "userdata";

        public GetStorageTokenController()
        {
            StorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["MS_AzureStorageAccountConnectionString"].ConnectionString);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        public string ConnectionString { get; }

        public CloudStorageAccount StorageAccount { get; }

        public CloudBlobClient BlobClient { get; }

        [HttpGet]
        public async Task<StorageTokenViewModel> GetAsync()
        {
            /*
            // The userId is the SID without the sid: prefix
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value.Substring(4);

            // Errors creating the storage container result in a 500 Internal Server Error
            var container = BlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            // Get the user directory within the container
            var directory = container.GetDirectoryReference(userId);

            */
            // Get the user directory within the container
            var container = BlobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            container.ListBlobs();


            var userId = "cd2c39696f8d5ca59639c940b7ad861b";
            var directory = container.GetDirectoryReference(userId);
            var blobName = "533445f0e34a4991b2add7a343533810";
            var blob = directory.GetBlockBlobReference(blobName);

            // Create a policy for accessing the defined blob
            var blobPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.Now.AddHours(2),
                Permissions = SharedAccessBlobPermissions.Read
                            | SharedAccessBlobPermissions.Write
                            | SharedAccessBlobPermissions.Create
            };

            return new StorageTokenViewModel
            {
                Name = blobName,
                Uri = blob.Uri,
                SasToken = blob.GetSharedAccessSignature(blobPolicy)
            };
        }
    }
}