namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;
public interface ITextGenerator
{
    Task<string> GeneratePostDescription(GeneratePostRequest request);

    Task<string> ReGeneratePostDescription(ReGeneratePostRequest request);

    Task<string> GeneratePromptForImageForPost(string productsAndServices);
}
