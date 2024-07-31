namespace Sameposty.Services.FacebookPixel;

public interface IFacebookPixelNotifier
{
    Task<string> NotifyNewPurchaseAsync(string userEmail);
}