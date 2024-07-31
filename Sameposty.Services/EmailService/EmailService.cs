using MailKit.Net.Smtp;
using MimeKit;
using Sameposty.Services.Configurator;
using Sameposty.Services.Secrets;

namespace Sameposty.Services.EmailService;
public class EmailService(ISecretsProvider secrets, IConfigurator configurator) : IEmailService
{
    private async Task SendEmail(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = body,
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
    public async Task SendWhyNotCreateAccountEmail(string message)
    {
        await SendEmail("jakubicki.m@gmail.com", "Powód dlaczego klient nie chce teraz zakładać konta:", message);
    }


    public async Task SentImageGeneratorErrorEmail(string errorMessage)
    {
        await SendEmail("jakubicki.m@gmail.com", "Error podczas tworzenia zdjęcia z AI", errorMessage);
    }

    public async Task EmailUserSubscriptionCreated(string to)
    {
        await SendEmail(to, "Dziękuję za zakup subskrypcji!", EmailBodyProvider.SubscriptionCreatedBodyEmail());
    }

    public async Task EmailUserSubscriptionDeleted(string to)
    {
        await SendEmail(to, "Subskrypcja dobiegła końca!", EmailBodyProvider.SubscriptionDeletedBodyEmail());
    }

    public async Task EmailUserNewPostsGenerated(string to)
    {
        await SendEmail(to, "Wygenerowaliśmy nowe posty. Sprawdź je koniecznie!", EmailBodyProvider.NewPostsCreatedBodyEmail());
    }

    public async Task SendNotifyUserPaymentFailedEmail(string to)
    {
        await SendEmail(to, "Niepowodzenie płatności za subskrypcję sameposty.pl", EmailBodyProvider.PaymentFailedEmailBody());
    }

    public async Task SendRegisterConfirmationEmail(string to, string token)
    {
        await SendEmail(to, "Potwierdź e-mail w serwisie sameposty.pl", EmailBodyProvider.RegisterConfirmationEmailBody(configurator.AngularClientBaseURl, token));
    }

    public async Task SendResetPasswordEmail(string to, string token, int userId)
    {
        await SendEmail(to, "Reset hasła w serwisie sameposty.pl", EmailBodyProvider.ResetPasswordEmailBody(configurator.AngularClientBaseURl, to, token, userId));
    }
}
