using azfun_organics.models;
using CosmosDB.SQLRepo.Contract;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace azfun_organics
{
    public class BatchTriggerEvent
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BatchEntity
    {
        static string GuidS => Guid.NewGuid().ToString();

        private readonly ISqlRepository<Batch> _service;
        private readonly HttpClient _client;

        public BatchEntity(ISqlRepository<Batch> service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _client = httpClientFactory.CreateClient();
            LastUpdate = DateTime.UtcNow;
            Value = new Batch
            {
                Files = new List<string>(),
                LastUpdate = LastUpdate,
                id = GuidS
            };
        }

        [JsonProperty("value")]
        public Batch Value { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime? LastUpdate { get; set; }

        public async Task Add(BatchTriggerEvent evt)
        {
            this.Value.Files.Add(evt.Name);
            this.Value.BatchId = evt.Key;
            this.LastUpdate = DateTime.UtcNow;
            this.Value.LastUpdate = this.LastUpdate;

            if (Value.Files.Count == 4)
            {
                await _service.Insert(Value);
                await Reset();
            }
        }

        public Task Reset()
        {
            Value = new Batch
            {
                Files = new List<string>(),
                LastUpdate = DateTime.UtcNow,
                id = GuidS
            };
            LastUpdate = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public Task<Batch> GetValue()
        {
            return Task.FromResult(this.Value);
        }

        public void Delete()
        {
            Entity.Current.DeleteState();
        }

        [FunctionName(nameof(BatchEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        {
            ctx.DispatchAsync<BatchEntity>();

            return Task.CompletedTask;
        }

    }
}
