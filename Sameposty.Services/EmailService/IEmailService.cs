namespace Sameposty.Services.EmailManager;
public interface IEmailService
{
    Task SentImageGeneratorErrorEmail(string errorMessage);
    Task EmailUserSubscriptionCreated(string to);
    Task EmailUserSubscriptionDeleted(string to);
    Task SendRegisterConfirmationEmail(string to, string token);
    Task SendResetPasswordEmail(string to, string token, int userId);
    Task EmailUserNewPostsGenerated(string to);
    Task SendNotifyUserPaymentFailedEmail(string to);
    Task SendWhyNotCreateAccountEmail(string message);
}