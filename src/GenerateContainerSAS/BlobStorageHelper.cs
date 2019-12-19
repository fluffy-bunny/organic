using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace GenerateContainerSAS
{
    public class BlobStorageHelper
    {
        public static async Task<CloudBlobContainer> GetContainer(string accountName, string accountKey, string container)
        {
            StorageCredentials storageCredentials = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(storageCredentials, true);

            var client = account.CreateCloudBlobClient();

            var c = client.GetContainerReference(container);
            await c.CreateIfNotExistsAsync();
            return c;
        }

        private static async Task<CloudBlobContainer> GetCloudBlobContainer(CloudBlobClient client, string container)
        {
            var c = client.GetContainerReference(container);
            await c.CreateIfNotExistsAsync();
            return client.GetContainerReference(container);
        }
        /*
        public static async Task<string> UploadFileAsync(Stream stream, string containerName)
        {
            var container = await GetContainer(containerName);

            string identifier = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            var fileBlob = container.GetBlockBlobReference(identifier);

            await fileBlob.UploadFromStreamAsync(stream);

            var blobSAS = GetBlobSasUri(container, identifier);

            return fileBlob.Uri.ToString();
        }
        */
        public static string GetContainerSasUri(CloudBlobContainer container, string storedPolicyName = null)
        {
            string sasContainerToken = GetContainerSasToken(container, storedPolicyName);
            return container.Uri + sasContainerToken;
        }
        public static string GetContainerSasToken(CloudBlobContainer container, string storedPolicyName = null)
        {
            string sasContainerToken;

            if (storedPolicyName == null)
            {
                SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
                };

                sasContainerToken = container.GetSharedAccessSignature(adHocPolicy, null);

            }
            else
            {
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);

            }

            return sasContainerToken;
        }

        private static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName = null)
        {
            string sasBlobToken;

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
                };

                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

            }
            else
            {
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

            }

            return blob.Uri + sasBlobToken;
        }
    }
}
