using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
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
        const string ContainerName = "quickstart-1235";

        static (string accountName, string primaryKey, string connectionString) GetAzureStorageCredentials()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCOUNT");
            var primaryKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_PRIMARY_KEY");
            return (accountName, primaryKey, connectionString);
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Blob storage v12 - .NET quickstart sample\n");
            (string accountName, string primaryKey, string connectionString) = GetAzureStorageCredentials();

            Console.WriteLine($"accountName:{accountName}");
            Console.WriteLine($"primaryKey:{primaryKey}");
            Console.WriteLine($"connectionString:{connectionString}");
            var container = await BlobStorageHelper.GetContainer(accountName, primaryKey, ContainerName);
            var sasContainerToken = BlobStorageHelper.GetContainerSasToken(container);

            Console.WriteLine($"sasContainerToken:{sasContainerToken}");

        }

    }
}
