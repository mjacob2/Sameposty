using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PublishPostFunction;

public class PublishPost(ILogger<PublishPost> logger)
{
    [Function(nameof(PublishPost))]
    public async Task Run(
        [ServiceBusTrigger($"%{FuncAppConstants.QueueName}%", Connection = "FunctionConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
