using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace azfun_organics
{
    public class AppSettingsFunctions
    {

        [FunctionName("GetAppSetting")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string settingName = req.Query["name"];
            if (string.IsNullOrEmpty(settingName))
                return new BadRequestObjectResult("Please pass a name on the query string");

            log.LogInformation($"Requesting setting {settingName}.");
            var value = Environment.GetEnvironmentVariable(settingName);
            return new OkObjectResult($"{settingName}={value}");
        }
    }
}
