using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using OpenAI.Interfaces;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
public class ImageGenerator(IOpenAIService openAiService) : IImageGenerator
{
    public async Task<string> GenerateImageUrl(string prompt)
    {
        var imageResult = await openAiService.Image.CreateImage(new ImageCreateRequest
        {
            Model = Models.Dall_e_3,
            Prompt = prompt,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size1024,
            ResponseFormat = "url",
            Quality = "hd",
            Style = "vivid",
        });

        var image = imageResult.Results.FirstOrDefault();

        return image.Url;
    }
}
