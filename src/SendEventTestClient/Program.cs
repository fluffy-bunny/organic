using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SendEventTestClient
{
    class Program
    {
        private static EventHubClient eventHubClient;
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
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(mySettings.EventHubConnectionString)
            {
                EntityPath = mySettings.EventHubName
            };

            await SendMessagesToEventHub(100);

            await eventHubClient.CloseAsync();

            return $" Hello { config["name"] } and {mySettings.Name} !";
        }
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
