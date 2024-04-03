using MailKit.Net.Smtp;
using MimeKit;
using Sameposty.Services.Configurator;

namespace Sameposty.Services.EmailService;
public class EmailService(EmailServiceSecrets secrets, IConfigurator configurator) : IEmailService
{
    public async Task SendNotifyInvalidSubscriptionEmail(string customerEmail, int customerId, int orderId)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sameposty.pl sy/sTEM", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", "info@sameposty.pl"));
        message.Subject = "Nowa subskrypcja na niewłaściwy e-mail!";

        var builder = new BodyBuilder
        {
            HtmlBody = $@"Ktoś właśnie kupił nową subskrypcję używając adresu {customerEmail} a my nie mamy takiego w bazie. Customer ID: {customerId}, Order ID: {orderId}. Skontaktuj się z operatorem płatności i wyjaśnij sprawę. Zobacz, może ktoś zrobił literówkę w mejlu i mamy podobny w bazie?"
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);

        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);

        var tt = await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendNotifyUserNewPostsCreatedEmail(string to)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = "Wygenerowaliśmy nowe posty. Sprawdź je koniecznie!";

        var builder = new BodyBuilder
        {
            HtmlBody = $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Kliknij poniższy przycisk i zobacz, jakie wspaniałe, nowe posty właśnie przygotowalismy! Pamiętaj, aby je zatwierdzić! <br><br>
<a href=https://sameposty.pl style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Sprawdź nowe posty</a>

<p>-- Marek z sameposty.pl<br>"
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);

        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);

        var tt = await client.SendAsync(message);
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
            HtmlBody = $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Witaj w serwisie sameposty.pl! <br>
Aby potwierdzić swoje konto i wygenerować pierwsze, wspaniałe posty na social media, kliknij w poniższy przycisk <br><br>
<a href={configurator.AngularClientBaseURl}/confirm?token={token} style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Potwierdź e-mail</a>

<p>-- Marek z sameposty.pl<br>"
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);

        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);

        var tt = await client.SendAsync(message);
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
            HtmlBody = $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Podobno chcesz zmienić hasło w serwisie sameposty.pl! <br>
Aby to zrobić, kliknij w poniższy przycisk <br><br>
<a href={configurator.AngularClientBaseURl}/update-password?token={token}&email={to}&id={userId} style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Resetuj hasło</a>

<p>-- Marek z sameposty.pl<br>"
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);

        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);

        var tt = await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
