using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public class ImageGeneratingOrchestrator(IImageGenerator imageGenerator, IImageSaver imageSaver) : IImageGeneratingOrchestrator
{
    public async Task<string> GenerateImage(string companyDescription)
    {
        var prompt = GeneratePrompt(companyDescription);

        var imageUrl = await imageGenerator.GenerateImageUrl(prompt);

        var imageName = await imageSaver.SaveImage(imageUrl);

        return imageName;
    }

    private static string GeneratePrompt(string companyDescription)
    {
        return $"For a small, local company, create an image or photograph, that will be used to post on their social media. This company is described by these words: {companyDescription}. Photo should be realistic and interesting, similar to what could be taken by this company.";
    }
}
