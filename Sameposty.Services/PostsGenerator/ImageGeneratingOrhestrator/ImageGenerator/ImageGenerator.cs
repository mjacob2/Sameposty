using Microsoft.Extensions.Logging;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
public class ImageGenerator(IOpenAIService openAiService, IConfigurator configurator, ILogger<ImageGenerator> logger, IEmailService email) : IImageGenerator
{
    public async Task<string> GenerateImageUrl(string myPrompt)
    {
        var imageRequest = new ImageCreateRequest
        {
            Model = Models.Dall_e_2,
            Prompt = myPrompt,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size256,
            ResponseFormat = "url"
        };


        //if (configurator.IsDevelopment)
        //{

        //    imageRequest.Model = Models.Dall_e_2;
        //    imageRequest.Prompt = prompt;
        //    imageRequest.N = 1;
        //    imageRequest.Size = StaticValues.ImageStatics.Size.Size512;
        //    imageRequest.ResponseFormat = "url";
        //}
        //else
        //{
        //    imageRequest.Model = Models.Dall_e_3;
        //    imageRequest.Prompt = prompt;
        //    imageRequest.N = 1;
        //    imageRequest.Size = StaticValues.ImageStatics.Size.Size1024;
        //    imageRequest.ResponseFormat = "url";
        //    imageRequest.Quality = "hd";
        //    imageRequest.Style = "vivid";
        //}

        var imageResult = await openAiService.Image.CreateImage(imageRequest);

        if (imageResult.Successful)
        {
            var result = imageResult.Results.FirstOrDefault();
            return result?.Url ?? "https://sameposty.pl/media/image-placeholder-gray.png";
        }

        if (imageResult.HeaderValues.All.TryGetValue("x-ratelimit-reset-images", out IEnumerable<string> resetValue))
        {
            var delayString = resetValue.FirstOrDefault();
            var delayInt = int.Parse(delayString);
            delayInt += 2;
            logger.LogWarning($"Rate limit reached, waiting for {delayInt} seconds");
            await Task.Delay(delayInt * 1000);
            return await GenerateImageUrl(myPrompt);
        }
        else
        {
            logger.LogError(imageResult.Error.Message ?? "Wystapił wyjątek else w ImageGenerator");
            await email.SentImageGeneratorErrorEmail(imageResult.Error.Message ?? "Wystapił wyjątek else w ImageGenerator");
            return "https://sameposty.pl/media/error.png";
        }
    }
}
