using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using Sameposty.Services.Configurator;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
public class ImageGenerator(IOpenAIService openAiService, IConfigurator configurator) : IImageGenerator
{
    public async Task<string> GenerateImageUrl(string prompt)
    {
        var imageRequest = new ImageCreateRequest();

        if (configurator.IsDevelopment)
        {

            imageRequest.Model = Models.Dall_e_2;
            imageRequest.Prompt = prompt;
            imageRequest.N = 1;
            imageRequest.Size = StaticValues.ImageStatics.Size.Size512;
            imageRequest.ResponseFormat = "url";
        }
        else
        {
            imageRequest.Model = Models.Dall_e_3;
            imageRequest.Prompt = prompt;
            imageRequest.N = 1;
            imageRequest.Size = StaticValues.ImageStatics.Size.Size1024;
            imageRequest.ResponseFormat = "url";
            imageRequest.Quality = "hd";
            imageRequest.Style = "vivid";
        }

        var imageResult = await openAiService.Image.CreateImage(imageRequest);

        var image = imageResult.Results.FirstOrDefault();

        return image.Url;
    }
}
