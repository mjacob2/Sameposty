using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsGenerator;
public interface IPostsGenerator
{
    Task<List<Post>> GenerateInitialPostsAsync(GeneratePostRequest request);

    Task<List<Post>> GeneratePremiumPostsAsync(GeneratePostRequest request);

    List<Post> GenerateStubbedPosts(GeneratePostRequest request);

    Task<Post> GeneratePost(GeneratePostRequest request);
}
