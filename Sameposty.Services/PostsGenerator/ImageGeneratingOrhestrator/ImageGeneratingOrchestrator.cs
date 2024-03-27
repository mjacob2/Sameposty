using Sameposty.DataAccess.Commands.Prompts;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public class ImageGeneratingOrchestrator(IImageGenerator imageGenerator, IImageSaver imageSaver, ITextGenerator textGenerator, ICommandExecutor commandExecutor) : IImageGeneratingOrchestrator
{
    public async Task<string> GenerateImage(string postDescription, int postId)
    {
        var prompt = await textGenerator.GeneratePromptForImageForPost(postDescription);

        await commandExecutor.ExecuteCommand(new AddPromptCommand()
        {
            Parameter = new DataAccess.Entities.Prompt()
            {
                ImagePrompt = prompt,
                PostId = postId
            }
        });

        var imageUrl = await imageGenerator.GenerateImageUrl(prompt);

        var imageName = await imageSaver.SaveImageFromUrl(imageUrl);

        return imageName;
    }
}
