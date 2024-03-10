using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public class ImageSaver(string wwwRootPath, HttpClient httpClient) : IImageSaver
{
    public async Task<string> SaveImageFromUrl(string imageUrl)
    {
        byte[] imageBytes = await DownloadImageAsync(imageUrl);

        using var imageStream = new MemoryStream(imageBytes);
        using var image = Image.Load(imageStream);

        string fileName = Guid.NewGuid().ToString();

        fileName += ".png";

        var encoder = new PngEncoder()
        {
            CompressionLevel = PngCompressionLevel.BestCompression,
        };

        string filePath = Path.Combine(wwwRootPath, fileName);

        image.Save(filePath, encoder);

        return fileName;
    }

    public async Task<string> SaveImageFromBytes(byte[] imageBytes, string fileExtension, CancellationToken ct)
    {
        string fileName = Guid.NewGuid().ToString();

        fileName += fileExtension;

        await SaveBytesToFileAsync(imageBytes, fileName);

        return fileName;
    }

    private async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        using var response = await httpClient.GetAsync(imageUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    private async Task SaveBytesToFileAsync(byte[] bytes, string fileName)
    {
        string filePath = Path.Combine(wwwRootPath, fileName);

        await File.WriteAllBytesAsync(filePath, bytes);
    }
}
