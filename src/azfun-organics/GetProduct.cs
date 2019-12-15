using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azfun_organics
{
    public class GetProduct
    {
        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string productId = string.IsNullOrEmpty(req.Query["productId"]) ? "defaultpid1234" : req.Query["productId"].ToString();

            var result = $"The product name for your product id {productId} is Starfruit Explosion";

            return (ActionResult)new OkObjectResult(result);
        }
    }
}
