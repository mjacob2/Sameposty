using Sameposty.Services.Configurator;

namespace Sameposty.Services.FileRemover;
public class FileRemover(IConfigurator configurator) : IFileRemover
{
    public void RemovePostImage(string fileName)
    {
        fileName = ExtractFilename(fileName);

        string filePath = Path.Combine(configurator.WwwRoot, fileName);

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

    private static string ExtractFilename(string urlString)
    {
        string[] parts = urlString.Split('/');

        string lastPart = parts[^1];

        string filenameWithoutExtension = lastPart.Replace("image.png", "");

        return filenameWithoutExtension + "image.png";
    }
}
