using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using CosmosDB.SQLRepo.Contract;
using azfun_organics.models;

namespace azfun_organics
{

    
   
    public class BlobStorageListener
    {
        private readonly ISqlRepository<Batch> _service;
        private readonly HttpClient _client;

        public BlobStorageListener(ISqlRepository<Batch> service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _client = httpClientFactory.CreateClient();
        }
 
        [FunctionName("BlobStorageListener")]
        public async Task Run(
            [BlobTrigger("testcontainer/{name}", Connection = "BlobStorageConnectionString")]Stream myBlob,
            string name,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            // Function input comes from the request content.
            var parts = name.Split(new char[] { '.' });
            var key = parts[0];
            var input = new BatchTriggerEvent
            {
                Name = name,
                Key = key
            };
            var entityId = new EntityId("BatchEntity", key);

            await client.SignalEntityAsync(entityId, "Add", input);

        }


    }
}
