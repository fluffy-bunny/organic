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
        private static bool SetRandomPartitionKey = false;
        private static string EventHubConnectionString = "Event Hubs namespace connection string";
        private static string EventHubName = "event hub name";

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
            EventHubName = mySettings.EventHubName;
            EventHubConnectionString = mySettings.EventHubConnectionString;

            // Creates an EventHubsConnectionStringBuilder object from a the connection string, and sets the EntityPath.
            // Typically the connection string should have the Entity Path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(100);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

            return $" Hello { config["name"] } and {mySettings.Name} !";
        }
        // Creates an Event Hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            var rnd = new Random();

            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";

                    // Set random partition key?
                    if (SetRandomPartitionKey)
                    {
                        var pKey = Guid.NewGuid().ToString();
                        await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)), pKey);
                        Console.WriteLine($"Sent message: '{message}' Partition Key: '{pKey}'");
                    }
                    else
                    {
                        await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                        Console.WriteLine($"Sent message: '{message}'");
                    }
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
