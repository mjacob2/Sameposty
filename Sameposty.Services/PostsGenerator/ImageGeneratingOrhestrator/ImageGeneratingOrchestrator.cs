using Sameposty.DataAccess.Executors;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public class ImageGeneratingOrchestrator(IImageGenerator imageGenerator, IImageSaver imageSaver, ITextGenerator textGenerator, ICommandExecutor commandExecutor) : IImageGeneratingOrchestrator
{
    private const string DefaultImageName = "default.png";

    public async Task<string> GenerateImage(string productsAndServices, int postId, bool generateImage)
    {
        if (!generateImage)
        {
            return DefaultImageName;
        }

        var prompt = await textGenerator.GeneratePromptForImageForPost(productsAndServices);

        //await commandExecutor.ExecuteCommand(new AddPromptCommand()
        //{
        //    Parameter = new DataAccess.Entities.Prompt()
        //    {
        //        ImagePrompt = prompt,
        //        PostId = postId
        //    }
        //});

        var imageUrl = await imageGenerator.GenerateImageUrl(prompt);

        var imageName = await imageSaver.SaveImageFromUrl(imageUrl);

        return imageName;
    }

    public async Task<string> GenerateImageFromUserPrompt(string prompt)
    {
        var imageUrl = await imageGenerator.GenerateImageUrl(prompt);
        var imageName = await imageSaver.SaveImageFromUrl(imageUrl);
        return imageName;
    }
}
