using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using TaskApi.AddTask;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddKafka(
    kafkaFlow => kafkaFlow
    .AddCluster(cluster =>
    {
        const string topicName = "tasks";
        cluster
        .WithBrokers(new[] { "localhost:7176" })
        .CreateTopicIfNotExists(topicName, numberOfPartitions: 1, replicationFactor: 1)
        .AddProducer(
            name: "publish-task",
            producer: producer => producer
            .DefaultTopic(topicName)
            .AddMiddlewares(middlewares => middlewares
            .AddSerializer<JsonCoreSerializer>())
            );
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/add", RequestHandler.HandleAsync).WithOpenApi();

app.Run();