using KafkaFlow;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApi.AddTask;

namespace Notifications
{
    internal class AddTaskHandler :IMessageHandler<AddTaskRequest>
    {
        private readonly ILogger<AddTaskHandler> logger;
        public AddTaskHandler(ILogger<AddTaskHandler> logger)
        {
            this.logger = logger;
        }
        public Task Handle(IMessageContext context, AddTaskRequest message)
        {
            if (message.DueDate.HasValue)
                logger.LogInformation("New Task {Title},{DueDate}", message.Title, message.DueDate);
            return Task.CompletedTask;
        }
    }
}
