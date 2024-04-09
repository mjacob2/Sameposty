using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.EasyCart;
public interface IEasyCart
{
    Task ActForSubcriptionCreated(EasyCartSubscriptionModel req, User userFromDb);
    Task ActForSubcriptionReneved(EasyCartSubscriptionModel req, User userFromDb);

    Task ActForSubcriptionCanceled(EasyCartSubscriptionModel req, User userFromDb);
}
