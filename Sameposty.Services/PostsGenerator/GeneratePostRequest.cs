namespace Sameposty.Services.PostsGenerator;
public class GeneratePostRequest
{
    public required int UserId { get; set; }

    public required string Branch { get; set; }

    public required string ProductsAndServices { get; set; }

    public required string Goals { get; set; }

    public required string Assets { get; set; }
}
