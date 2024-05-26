using FastEndpoints;
using Sameposty.Services.EmailService;

namespace Sameposty.API.Endpoints.Utilities.SendEmail;

public class SendEmailEndpoint(IEmailService email) : Endpoint<SendEmailRequest>
{
    public override void Configure()
    {
        Post("send-email");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SendEmailRequest req, CancellationToken ct)
    {
        await email.SendWhyNotCreateAccountEmail(req.Message);
        await SendOkAsync(ct);
    }
}
