namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public interface IImageSaver
{
    Task<string> SaveImageFromUrl(string imageUrl);

    Task<string> SaveImageFromBytes(byte[] imageBytes, string fileExtension, CancellationToken ct);

    Task<string> DownsizePNG(string imageUrl);
}
