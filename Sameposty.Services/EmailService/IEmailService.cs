namespace Sameposty.Services.EmailService;
public interface IEmailService
{
    Task SendRegisterConfirmationEmail(string to, string token);
    Task SendResetPasswordEmail(string to, string token, int userId);
    Task EmailUserNewPostsGenerated(string to);
    Task SendNotifyUserSubscriptionCanceledPaymentFailedEmail(string to);
}
