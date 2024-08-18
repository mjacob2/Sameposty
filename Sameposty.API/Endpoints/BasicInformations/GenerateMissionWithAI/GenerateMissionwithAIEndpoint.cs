using FastEndpoints;
using Sameposty.Services.PostsGenerator.ImageGeneratingOrhestrator.TextGenerator;

namespace Sameposty.API.Endpoints.BasicInformations.GenerateMissionWithAI;

public class GenerateMissionwithAIEndpoint(ITextGenerator textGenerator) : Endpoint<GenerateMissionWithAIRequest>
{
    public override void Configure()
    {
        Post("generateMissionWithAI");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GenerateMissionWithAIRequest req, CancellationToken ct)
    {
        var request = new GenerateMissionRequest
        {
            ProductsAndServices = req.ProductsAndServices,
            Audience = req.Audience,
            Goals = req.Goals,
            Assets = req.Assets
        };

        var mission = await textGenerator.GenerateCompanyMission(request);


        await SendOkAsync(mission, ct);
    }
}
