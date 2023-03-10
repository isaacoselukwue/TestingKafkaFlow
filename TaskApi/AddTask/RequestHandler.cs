using KafkaFlow.Producers;

namespace TaskApi.AddTask
{
    public class RequestHandler
    {
        public static async Task<IResult> HandleAsync(
            IProducerAccessor producerAccessor,
            AddTaskRequest request, CancellationToken cancellationToken)
        {
            var producer = producerAccessor.GetProducer("publish-task");
            await producer.ProduceAsync(
                "key",
                request
                );
            return Results.Accepted();
        }
    }
}
