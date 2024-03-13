namespace Sameposty.Services.FileRemover;
public class FileRemover(string webRootPath) : IFileRemover
{
    public void RemovePostImage(string fileName)
    {
        try
        {
            fileName = ExtractFilename(fileName);

            string filePath = Path.Combine(webRootPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"File '{fileName}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"File '{fileName}' does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    private static string ExtractFilename(string urlString)
    {
        string[] parts = urlString.Split('/');

        string lastPart = parts[^1];

        string filenameWithoutExtension = lastPart.Replace("image.png", "");

        return filenameWithoutExtension + "image.png";
    }
}
