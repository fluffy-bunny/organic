using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CosmosDB.SQLRepo.Contract;
using azfun_organics.models;
using System.Net.Http;
using System.Linq.Expressions;
using System.Linq;

namespace azfun_organics
{
    public class GetRatings
    {
        private readonly ISqlRepository<RatingRecord> _service;
        private readonly HttpClient _client;

        public GetRatings(ISqlRepository<RatingRecord> service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _client = httpClientFactory.CreateClient();
        }
        static string GuidS => Guid.NewGuid().ToString();

        [FunctionName("GetRatings")]
        public async Task<IActionResult> Run(
              [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
              ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(req.Query["userId"]))
            {
                return new NotFoundObjectResult("Does not exit");
            }

            string userId = req.Query["userId"].ToString();

            Expression<Func<RatingRecord, bool>> lambda = x => x.UserId == userId;

            var result = await _service.Get(lambda);
            if (!result.Any())
            {
                return new NotFoundObjectResult("Does not exit");
            }
            var jsonResult = new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };

            return jsonResult;
        }
    }
}
