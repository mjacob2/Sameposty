using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;

namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator;
public class ImageGeneratingOrchestrator(IImageGenerator imageGenerator, IImageSaver imageSaver) : IImageGeneratingOrchestrator
{
    public async Task<string> GenerateImage(GeneratePostRequest request)
    {
        var prompt = GeneratePrompt(request);

        var imageUrl = await imageGenerator.GenerateImageUrl(prompt);

        var imageName = await imageSaver.SaveImage(imageUrl);

        return imageName;
    }

    private static string GeneratePrompt(GeneratePostRequest request)
    {
        return $"Stwórz zdjęcie jakie zostało wykonane w firmie aparatem fotograficznymw w małej, lokalnej firmie i w celu opublikowania ma social media dla firmy. Firma jest z branży: {request.Branch}, sprzedaje lub śiadczy takie usługi usług: {request.ProductsAndServices}, cele jakimi kieruje się firma {request.Goals} oraz jakie czynniki wyróżniają tę firmę na rynku: {request.Assets}. Zdjęcie powinno też zachęcać do skorzystania z usług tej firmy. ";
    }
}
