namespace Sameposty.API.Endpoints.Posts.GetPosts;

public class GetPostsResponse
{
    public List<Post> Posts { get; set; } = [];

    public GetPostsResponse(List<DataAccess.Entities.Post> postsEntities)
    {
        foreach (var item in postsEntities)
        {
            Posts.Add(new Post
            {
                Id = item.Id,
            });
        }
    }

}


public class Post
{
    public int Id { get; set; }

    public string Title { get; set; }
}