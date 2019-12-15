using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using CosmosDB.SQLRepo.Contract;
using azfun_organics.models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace azfun_organics
{


    public class CreateRating
    {
        private readonly ISqlRepository<Ratings> _service;
        private readonly HttpClient _client;

        public CreateRating(ISqlRepository<Ratings> service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _client = httpClientFactory.CreateClient();
        }
        static string GuidS => Guid.NewGuid().ToString();

        [FunctionName("CreateRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req,  
            ILogger log)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Ratings>(requestBody);
            input.id = GuidS;
            input.Timestamp = DateTime.UtcNow;

            await _service.Insert(input);


            string productId = string.IsNullOrEmpty(req.Query["productId"]) ? "defaultpid1234" : req.Query["productId"].ToString();

            var result = $"The product name for your product id {productId} is Starfruit Explosion";

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
