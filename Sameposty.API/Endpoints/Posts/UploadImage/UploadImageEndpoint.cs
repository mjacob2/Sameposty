using FastEndpoints;
using Sameposty.DataAccess.Commands.Posts;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FileRemover;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageSaver;

namespace Sameposty.API.Endpoints.Posts.UploadImage;

public class UploadImageEndpoint(IImageSaver imageSaver, IHttpContextAccessor httpContextAccessor, ICommandExecutor commandExecutor, IFileRemover fileRemover) : Endpoint<UploadImageRequest>
{
    public override void Configure()
    {
        Post("postsimages");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadImageRequest req, CancellationToken ct)
    {
        if (req.ImageData == null || req.ImageData.Length == 0)
        {
            throw new ArgumentNullException("No image to save");
        }

        using var memoryStream = new MemoryStream();
        await req.ImageData.CopyToAsync(memoryStream, ct);

        var fileBytes = memoryStream.ToArray();
        var imageName = await imageSaver.SaveImageFromBytes(fileBytes, req.ImageData.FileName, ct);

        var baseApiUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
        var imageUrl = $"{baseApiUrl}/{imageName}";
        var command = new UpdatePostImageUrlCommand(req.PostId, imageUrl);
        await commandExecutor.ExecuteCommand(command);

        if (!string.IsNullOrEmpty(req.OldImageUrl))
        {
            fileRemover.RemovePostImage(req.OldImageUrl);
        }

        await SendOkAsync(imageUrl, ct);
    }
}
