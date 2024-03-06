namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public interface IImageGeneratingOrchestrator
{
    Task<string> GenerateImage(GeneratePostRequest request);
}
