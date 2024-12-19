using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostGeneratingManager;
public interface IPostGeneratingManager
{
    Task<Post> GenerateSinglePost(User user, DateTime date, bool generateText, bool generateImage);
    Task<List<Post>> GenerateNumberOfPosts(User user, int numberOfPostsToGenerate);
}
