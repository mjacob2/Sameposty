using Sameposty.Services.Configurator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public class ImageSaver(IConfigurator configurator, HttpClient httpClient) : IImageSaver
{
    public async Task<string> SaveImageFromUrl(string imageUrl)
    {
        byte[] imageBytes = await DownloadImageAsync(imageUrl);

        using var imageStream = new MemoryStream(imageBytes);
        using var image = Image.Load(imageStream);

        string fileName = Guid.NewGuid().ToString();

        fileName += "image.png";

        var encoder = new PngEncoder()
        {
            CompressionLevel = PngCompressionLevel.BestCompression,
        };

        //var encoder = new JpegEncoder()
        //{
        //    Quality = 70, // Adjust quality level (0-100), lower value means more compression
        //};


        string filePath = Path.Combine(configurator.WwwRoot, fileName);

        image.Save(filePath, encoder);

        return fileName;
    }

    public async Task<string> DownsizePNG(string imageUrl)
    {
        byte[] imageBytes = await DownloadImageAsync(imageUrl);
        using var imageStream = new MemoryStream(imageBytes);
        using var image = Image.Load(imageStream);
        image.Mutate(x => x
        .Resize(50, 50, KnownResamplers.Lanczos3)
        .Crop(new Rectangle(0, 0, 50, 50)));


        string fileName = Guid.NewGuid().ToString();

        fileName += "image.png";
        string filePath = Path.Combine(configurator.WwwRoot, "Thumbnails", fileName);

        image.Save(filePath);

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
        string filePath = Path.Combine(configurator.WwwRoot, fileName);

        await File.WriteAllBytesAsync(filePath, bytes);
    }
}
