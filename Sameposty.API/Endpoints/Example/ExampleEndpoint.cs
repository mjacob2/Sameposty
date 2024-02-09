using FastEndpoints;
using Sameposty.Services.ExampleS;

namespace Sameposty.API.Endpoints.Example;

public class ExampleEndpoint(IExample example) : Endpoint<GetExampleRequest, ExampleResponse>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("example/{number}");
    }

    public override async Task HandleAsync(GetExampleRequest req, CancellationToken ct)
    {
        var gg = example.DoSome();

        var response = new ExampleResponse
        {
            ResponseNumber = gg.Length,
        };

        await SendAsync(response, cancellation: ct);
    }
}
