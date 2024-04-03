namespace Sameposty.Services.EmailService;
public interface IEmailService
{
    Task SendRegisterConfirmationEmail(string to, string token);
    Task SendResetPasswordEmail(string to, string token, int userId);
    Task SendNotifyInvalidSubscriptionEmail(string customerEmail, int customerId, int orderId);
    Task SendNotifyUserNewPostsCreatedEmail(string to);
}
