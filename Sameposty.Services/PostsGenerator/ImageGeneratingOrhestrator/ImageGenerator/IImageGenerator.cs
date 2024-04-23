namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
public interface IImageGenerator
{
    Task<string> GenerateImageUrl(string myPrompt);
}
