namespace Sameposty.Services.Tests;

public class ImageSaverTests
{
    [Fact]
    public async Task DownsizePNG_WhenGivenValidImageUrl_ReturnsResizedImagePath()
    {
        // Arrange
        string wwwRootPath = "C:\\Users\\jakub\\source\\repos\\Sameposty\\Sameposty.API\\wwwroot\\";
        var httpClient = new HttpClient();
        //var service = new ImageSaver(wwwRootPath, httpClient);
        //string imageUrl = "https://sameposty-api.azurewebsites.net/abb6a2bd-f7ec-4c74-8fbe-4cc5b3acb918image.png";

        // Act
        //string resizedImagePath = await service.DownsizePNG(imageUrl);

        // Assert
        // Assert.NotNull(resizedImagePath);
        //Assert.True(File.Exists(resizedImagePath));

        // Clean up
        // File.Delete(resizedImagePath);
    }
}