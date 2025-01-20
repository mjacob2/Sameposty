namespace Sameposty.Services.Email;
public interface IEmailService
{
    Task SentImageGeneratorErrorEmail(string errorMessage);
    Task EmailUserSubscriptionCreated(string to);
    Task EmailUserSubscriptionDeleted(string to);
    Task SendRegisterConfirmationEmail(string to, string token);
    Task SendResetPasswordEmail(string to, string token, int userId);
    Task SubscriptionPaid(string to);
    Task SendNotifyUserPaymentFailedEmail(string to);
    Task SendWhyNotCreateAccountEmail(string message);
}