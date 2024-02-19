namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public interface IImageSaver
{
    Task<string> SaveImage(string imageUrl);
}
