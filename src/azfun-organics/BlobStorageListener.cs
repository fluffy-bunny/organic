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

    class BlobEntity
    {
        public BlobEntity()
        {
            Files = new List<string>();
        }
        public List<string> Files { get; set; }
    }
    class BlobTriggerEvent
    {
        public string Name { get; internal set; }
        public string Key { get; internal set; }
    }
    public class BlobStorageListener
    {
        private readonly ISqlRepository<Batch> _service;
        private readonly HttpClient _client;

        public BlobStorageListener(ISqlRepository<Batch> service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _client = httpClientFactory.CreateClient();
        }
        static string GuidS => Guid.NewGuid().ToString();

        [FunctionName("BlobAccumulator")]
        public async Task BlobAccumulator([EntityTrigger] IDurableEntityContext ctx)
        {
            var entity = ctx.GetState<BlobEntity>();
            if (entity == null)
            {
                entity = new BlobEntity();
                ctx.SetState(entity);
            }
            var input = ctx.GetInput<BlobTriggerEvent>();
            switch (ctx.OperationName.ToLowerInvariant())
            {
                case "add":
                    entity.Files.Add(input.Name);
                    break;
                case "reset":
                    entity.Files.Clear();
                    break;
                case "get":
                    ctx.Return(entity);
                    break;
            }

            ctx.SetState(entity);
            if (entity.Files.Count == 3)
            {
                var batch = new Batch
                {
                    id = GuidS,
                    Timestamp = DateTime.UtcNow,
                    Files = entity.Files,
                    BatchId = input.Key
                };
                await _service.Insert(batch);
            }
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
            var input = new BlobTriggerEvent
            {
                Name = "test",
                Key = key
            };

            var entityId = new EntityId(nameof(BlobAccumulator), key);

            await client.SignalEntityAsync(entityId, "Add", new { Name = name, Key = key });

        }


    }
}
