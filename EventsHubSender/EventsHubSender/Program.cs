using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using System.Web.Script.Serialization;

namespace EventsHubSender
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EhConnectionString = "Endpoint=sb://mtehtest-ns.servicebus.windows.net/;SharedAccessKeyName=Lab;SharedAccessKey=rV8e7G9lTCN0eD2ldWC574DtcaDO2Ah5ae3MQ8XuLUk=";
        private const string EhEntityPath = "mtehtest";
       private static readonly Random rnd1 = new Random();
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
            {
                EntityPath = EhEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            int a = 1000;
            do
            {
                var telemetryDataPoint = new
                {
                    SensorId = 104,
                    Location = "Malta",
                    Speed = rnd1.Next(0, 100),
                    time = DateTime.Now

                };
                var messageString = new JavaScriptSerializer().Serialize(telemetryDataPoint);

                await SendMessagesToEventHub(messageString);

                

                Console.WriteLine("sending msg {0}", a);
                a--;
              
            } while (a > 0);

            await eventHubClient.CloseAsync();
        }

        // Creates an Event Hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(string message)
        {
            try
                {
                    DateTime curtime= DateTime.Now;
                    //var message = string.Concat($"Message {i} @", curtime.ToLongTimeString());
                    //Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

               // await Task.Delay(1);

            //Console.WriteLine($"{numMessagesToSend} messages sent.");
        }

    }
}
