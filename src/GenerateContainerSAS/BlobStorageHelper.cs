
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
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
        public static async Task<string> GetAccessToken(string tenantId, string clientId, string clientSecret)
        {
            var authContext = new AuthenticationContext($"https://login.windows.net/{tenantId}");
            var credential = new ClientCredential(clientId, clientSecret);
            var result = await authContext.AcquireTokenAsync("https://storage.azure.com", credential);

            if (result == null)
            {
                throw new Exception("Failed to authenticate via ADAL");
            }

            return result.AccessToken;
        }
        public static async Task<string> GetContainerSasTokenAsync(string storageAccountName, string containerName, string access_token)
        {
            TokenCredential tokenCredential = new Microsoft.Azure.Storage.Auth.TokenCredential(access_token);
            StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
            CloudBlobClient client = new CloudBlobClient(new Uri($"https://{storageAccountName}.blob.core.windows.net"), storageCredentials);
            CloudBlobContainer container = client.GetContainerReference(containerName);

            var delegationKey = await client.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(15));
            var sas = container.GetUserDelegationSharedAccessSignature(delegationKey, new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow.AddHours(-1),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(16),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
            });

            return sas;
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
