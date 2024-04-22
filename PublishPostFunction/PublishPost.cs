using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;

namespace PublishPostFunction;

public class PublishPost(ILogger<PublishPost> logger, ICommandExecutor commandExecutor, IFileRemover fileRemover, IImageSaver imageSaver)
{
    [Function(nameof(PublishPost))]
    public async Task Run(
        [ServiceBusTrigger($"%{FuncAppConstants.QueueName}%", Connection = "FunctionConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {

        await messageActions.CompleteMessageAsync(message);
    }
}
