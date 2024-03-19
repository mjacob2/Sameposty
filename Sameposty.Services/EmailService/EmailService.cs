using MailKit.Net.Smtp;
using MimeKit;

namespace Sameposty.Services.EmailService;
public class EmailService(EmailServiceSecrets secrets) : IEmailService
{
    public async Task SendRegisterConfirmationEmail(string to)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Marek z sameposty.pl", "info@sameposty.pl"));
        message.To.Add(new MailboxAddress("jacek", to));
        message.Subject = "Potwierdź e-mail w serwisie sameposty.pl";
        message.Body = new TextPart("plain")
        {
            Text = @$"Cześć!,

Jesli to Ty właśnie zakładasz konto w serwisie sameposty.pl, kliknij w poniższy link aby potwierdzić, że adres email {to} należy do Ciebie.

LINK


-- Marek z sameposty.pl"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("s6.cyber-folks.pl", 465, true);

        // Note: only needed if the SMTP server requires authentication
        await client.AuthenticateAsync("info@sameposty.pl", secrets.EmailInfoPassword);

        var tt = await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
