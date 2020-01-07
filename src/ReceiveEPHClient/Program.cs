using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ReceiveEPHClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var helloWorld = await GetHelloWorldAsync();
            Console.WriteLine(helloWorld);

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();

        }
        static async Task<string> GetHelloWorldAsync()
        {
            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .AddEnvironmentVariables();

            builder.AddUserSecrets<Program>();

            var config = builder.Build();

            var mySettings = config.GetSection("mySettings").Get<MySettings>();
            Console.WriteLine($"EventHubName:{mySettings.EventHubName}");
            Console.WriteLine($"EventHubConnectionString:{mySettings.EventHubConnectionString}");
            Console.WriteLine($"StorageContainerName:{mySettings.StorageContainerName}");
            Console.WriteLine($"StorageAccountName:{mySettings.StorageAccountName}");
            Console.WriteLine($"StorageAccountKey:{mySettings.StorageAccountKey}");
            Console.WriteLine($"StorageConnectionString:{mySettings.StorageConnectionString}");


            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                mySettings.EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                mySettings.EventHubConnectionString,
                mySettings.StorageConnectionString,
                mySettings.StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();

            return $" Hello { config["name"] } and {mySettings.Name} !";
        }
    }
}
