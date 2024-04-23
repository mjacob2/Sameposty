using FastEndpoints;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.ImageGenerator;

namespace Sameposty.API.Endpoints.TestEndpoints;

public class ImageTestEndpoint(IImageGenerator imageGenerator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("test");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {

        var gg = await imageGenerator.GenerateImageUrl("zielone jabłko");


        await SendOkAsync(gg, ct);
    }
}
