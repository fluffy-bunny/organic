using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Storage.Net;
using Storage.Net.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GenerateContainerSAS
{
    class Program
    {
        const string ContainerName = "testcontainer";

        static (string accountName, string primaryKey, string connectionString, string tenantId, string servicePrincipalId, string servicePrincipalPassword) GetAzureStorageCredentials()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT");
            var primaryKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_PRIMARY_KEY");
            var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");
            var servicePrincipalId = Environment.GetEnvironmentVariable("AZURE_SERVICE_PRINCIPAL_ID");
            var servicePrincipalPassword = Environment.GetEnvironmentVariable("AZURE_SERVICE_PRINCIPAL_PASSWORD");


            return (accountName, primaryKey, connectionString, tenantId, servicePrincipalId, servicePrincipalPassword);
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Blob storage v12 - .NET quickstart sample\n");
            (string accountName, string primaryKey, string connectionString, string tenantId, string servicePrincipalId, string servicePrincipalPassword) = GetAzureStorageCredentials();

            Console.WriteLine($"accountName:{accountName}");
            Console.WriteLine($"primaryKey:{primaryKey}");
            Console.WriteLine($"connectionString:{connectionString}");
            Console.WriteLine($"tenantId:{tenantId}");
            Console.WriteLine($"servicePrincipalId:{servicePrincipalId}");
            Console.WriteLine($"servicePrincipalPassword:{servicePrincipalPassword}");
            var container = await BlobStorageHelper.GetContainer(accountName, primaryKey, ContainerName);
            var sasContainerToken = BlobStorageHelper.GetContainerSasToken(container);

            Console.WriteLine($"\n--- via connection-string ---");
            Console.WriteLine($"{sasContainerToken}");

            var accessToken = await BlobStorageHelper.GetAccessToken(tenantId, servicePrincipalId, servicePrincipalPassword);
            sasContainerToken = await BlobStorageHelper.GetContainerSasTokenAsync(accountName, ContainerName, accessToken);
            Console.WriteLine($"\n--- via service principal ---");
            Console.WriteLine($"{sasContainerToken}");
        }

    }
}
