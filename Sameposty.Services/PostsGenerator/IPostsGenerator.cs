using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsGenerator;
public interface IPostsGenerator
{
    Task<List<Post>> GenerateInitialPostsAsync(int userId, string companyDescription);
}
