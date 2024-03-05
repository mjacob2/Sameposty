namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;
public class ImageSaver(string wwwRootPath, HttpClient httpClient) : IImageSaver
{
    public async Task<string> SaveImage(string imageUrl)
    {
        byte[] imageBytes = await DownloadImageAsync(imageUrl);

        string fileName = Guid.NewGuid().ToString();

        fileName += ".png";

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
