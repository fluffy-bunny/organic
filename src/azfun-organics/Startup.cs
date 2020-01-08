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

            // at this point it looks like local.settings.json is loaded.
            var config = new ConfigurationBuilder()
                .SetBasePath(actual_root)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .Build();
            foreach (var item in config.AsEnumerable())
            {
                Environment.SetEnvironmentVariable(item.Key, item.Value);
            }
            /*
            var value = Environment.GetEnvironmentVariable("snow_agent");
            value = Environment.GetEnvironmentVariable("secretOrganics");

*/

            var endPointUri = Environment.GetEnvironmentVariable(SqlConfig<RatingRecord>.EnvironmentNames.EndPointUri);
            var primaryKey = Environment.GetEnvironmentVariable(SqlConfig<RatingRecord>.EnvironmentNames.PrimaryKey);

            builder.Services.AddSingleton<ISqlConfig<RatingRecord>>((s) =>
            {
                return new SqlConfig<RatingRecord>(endPointUri, primaryKey, "organics", "ratings", "/productId");
            });

            builder.Services.AddSingleton<ISqlConfig<Batch>>((s) =>
            {
                return new SqlConfig<Batch>(endPointUri, primaryKey, "organics", "batch", "/batchId");

            });
            builder.Services.AddSingleton<ISqlRepository<RatingRecord>, SqlRepository<RatingRecord>>();
            builder.Services.AddSingleton<ISqlRepository<Batch>, SqlRepository<Batch>>();

            builder.Services.AddHttpClient();
        }
    }
}
