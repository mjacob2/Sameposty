using Sameposty.DataAccess.Executors;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageSaver;
using Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator;
public class ImageGeneratingOrchestrator(IImageGenerator imageGenerator, IImageSaver imageSaver, ITextGenerator textGenerator, ICommandExecutor commandExecutor) : IImageGeneratingOrchestrator
{
    public async Task<string> GenerateImage(string productsAndServices, int postId, bool generateImage)
    {
        if (!generateImage)
        {
            return AppConstants.DefaultImageName;
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
