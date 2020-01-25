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
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;

namespace azfun_organics
{
    public class GetKeyVaultSecret
    {
        private readonly HttpClient _client;

        public GetKeyVaultSecret(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }
        static string GuidS => Guid.NewGuid().ToString();

        [FunctionName("GetKeyVaultSecret")]
        public async Task<IActionResult> Run(
              [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
              ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            if (string.IsNullOrEmpty(req.Query["secret"]))
            {
                return new NotFoundObjectResult("Does not exit");
            }
            string secretKey = req.Query["secret"].ToString();

            var secretValue = "nothing.";
            int retries = 0;
            bool retry = false;
            try
            {
                /* The next four lines of code show you how to use AppAuthentication library to fetch secrets from your key vault */
                AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var secret = await keyVaultClient.GetSecretAsync("https://kv-organics.vault.azure.net/secrets/AppSecret")
                        .ConfigureAwait(false);
                secretValue = secret.Value;
            }
            /* If you have throttling errors see this tutorial https://docs.microsoft.com/azure/key-vault/tutorial-net-create-vault-azure-web-app */
            /// <exception cref="KeyVaultErrorException">
            /// Thrown when the operation returned an invalid status code
            /// </exception>
            catch (KeyVaultErrorException keyVaultException)
            {
                secretValue = keyVaultException.Message;
            }


            var jsonResult = new JsonResult(secretValue)
            {
                StatusCode = StatusCodes.Status200OK
            };
          
            return jsonResult;
             
        }
        // This method implements exponential backoff if there are 429 errors from Azure Key Vault
        private long getWaitTime(int retryCount)
        {
            long waitTime = ((long)Math.Pow(2, retryCount) * 100L);
            return waitTime;
        }

        // This method fetches a token from Azure Active Directory, which can then be provided to Azure Key Vault to authenticate
        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");
            return accessToken;
        }
    }
}
