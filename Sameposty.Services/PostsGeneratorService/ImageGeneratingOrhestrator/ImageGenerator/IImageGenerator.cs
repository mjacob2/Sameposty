namespace Sameposty.Services.PostsGeneratorService.ImageGeneratingOrhestrator.ImageGenerator;
public interface IImageGenerator
{
    Task<string> GenerateImageUrl(string myPrompt);
}
