namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
public interface ITextGenerator
{
    Task<string> GeneratePostDescription(GeneratePostRequest request);

    Task<string> GeneratePromptForImageForPost(string productsAndServices);
}
