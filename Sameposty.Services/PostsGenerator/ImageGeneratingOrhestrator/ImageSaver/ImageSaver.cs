using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public class ImageSaver(string wwwRootPath, HttpClient httpClient) : IImageSaver
{
    public async Task<string> SaveImageFromUrl(string imageUrl)
    {
        byte[] imageBytes = await DownloadImageAsync(imageUrl);

        using var imageStream = new MemoryStream(imageBytes);
        using var image = Image.Load(imageStream);

        string fileName = Guid.NewGuid().ToString();

        fileName += "image.jpg";

        //var encoder = new PngEncoder()
        //{
        //    CompressionLevel = PngCompressionLevel.BestCompression,
        //};

        var encoder = new JpegEncoder()
        {
            Quality = 70, // Adjust quality level (0-100), lower value means more compression
        };


        string filePath = Path.Combine(wwwRootPath, fileName);

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


        string fileName = Guid.NewGuid().ToString() + ".png";
        string folderPath = Path.Combine(wwwRootPath, "Thumbnails");
        Directory.CreateDirectory(folderPath);
        string resizedImagePath = Path.Combine(folderPath, fileName);

        await image.SaveAsync(resizedImagePath);

        return resizedImagePath;
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
