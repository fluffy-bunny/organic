using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using CosmosDB.SQLRepo;
using CosmosDB.SQLRepo.Contract;
using azfun_organics.models;
using System.Configuration;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(azfun_organics.Startup))]
namespace azfun_organics
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var actual_root = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot")  // local_root
                    ?? (Environment.GetEnvironmentVariable("HOME") == null
                        ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        : $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot"); // azure_root

           

            var endPointUri = Environment.GetEnvironmentVariable(SqlConfig<Ratings>.EnvironmentNames.EndPointUri);
            var primaryKey = Environment.GetEnvironmentVariable(SqlConfig<Ratings>.EnvironmentNames.PrimaryKey);

            builder.Services.AddSingleton<ISqlConfig<Ratings>>((s) =>
            {
                return new SqlConfig<Ratings>(endPointUri, primaryKey, "organics", "ratings", "/productId");
            });

            builder.Services.AddSingleton<ISqlConfig<Batch>>((s) =>
            {
                return new SqlConfig<Batch>(endPointUri, primaryKey, "organics", "batch", "/batchId");

            });
            builder.Services.AddSingleton<ISqlRepository<Ratings>, SqlRepository<Ratings>>();
            builder.Services.AddSingleton<ISqlRepository<Batch>, SqlRepository<Batch>>();

            builder.Services.AddHttpClient();
        }
    }
}
