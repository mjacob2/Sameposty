namespace Sameposty.Services.EmailService;
public interface IEmailService
{
    Task SendRegisterConfirmationEmail(string to, string token);
}
