using MailKit.Net.Smtp;
using MimeKit;
using Sameposty.Services.Configurator;

namespace Sameposty.Services.EmailService;
public class EmailService(EmailServiceSecrets secrets, IConfigurator configurator) : IEmailService
{
    public async Task EmailUserNewPostsGenerated(string to)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = "Wygenerowaliśmy nowe posty. Sprawdź je koniecznie!";
        var builder = new BodyBuilder
        {
            HtmlBody = EmailBodyProvider.NewPostsCreatedBodyEmail(),
        };
        message.Body = builder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendNotifyUserSubscriptionCanceledPaymentFailedEmail(string to)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = "Niepowodzenie płatności za subskrypcję sameposty.pl";
        var builder = new BodyBuilder
        {
            HtmlBody = EmailBodyProvider.SubscriptionCanceledPaymentFailedEmailBody(),
        };
        message.Body = builder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendRegisterConfirmationEmail(string to, string token)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = "Potwierdź e-mail w serwisie sameposty.pl";
        var builder = new BodyBuilder
        {
            HtmlBody = EmailBodyProvider.RegisterConfirmationEmailBody(configurator.AngularClientBaseURl, token),
        };

        message.Body = builder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendResetPasswordEmail(string to, string token, int userId)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = "Reset hasła w serwisie sameposty.pl";

        var builder = new BodyBuilder
        {
            HtmlBody = EmailBodyProvider.ResetPasswordEmailBody(configurator.AngularClientBaseURl, to, token, userId),
        };

        message.Body = builder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
