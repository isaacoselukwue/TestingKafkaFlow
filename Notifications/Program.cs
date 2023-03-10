using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Notifications
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            const string topic = "tasks";
            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            services.AddKafkaFlowHostedService(
                kafka => kafka
                .UseMicrosoftLog()
                .AddCluster(cluster =>
                {
                    cluster.WithBrokers(new[] { "localhost:7176" })
                    .AddConsumer(consumer =>
                    consumer
                    .Topic(topic)
                    .WithGroupId("notifications")
                    .WithBufferSize(100)
                    .WithWorkersCount(3)
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .AddMiddlewares(middlewares => middlewares
                    .AddSerializer<JsonCoreSerializer>()
                    .AddTypedHandlers(handlers =>
                    handlers.AddHandler<AddTaskHandler>()
                        )
                    )
                    );
                }
                    )
                );

            var provider = services.BuildServiceProvider();

            var bus = provider.CreateKafkaBus();
            await bus.StartAsync();

            Console.WriteLine("Hello, World!");
        }
    }
}