namespace Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.PostDescriptionGenerator;
public interface IPostDescriptionGenerator
{
    Task<string> GeneratePostDescription(string companyDescription);
}
