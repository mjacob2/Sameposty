namespace Sameposty.Services.FacebookPixel;

public interface IFacebookPixelNotifier
{
    Task<string> NotifyNewLeadAsync(string userEmail);
}