using Sameposty.DataAccess.Entities;
using Sameposty.Services.DTOs;

namespace Sameposty.DataAccess.Mappings;
public static class UserMappings
{
    public static UserBasicInfo MapToUserBasicInfo(this User user)
    {
        string name = user.Name;
        const string companySuffix = "SPÓŁKA Z OGRANICZONĄ ODPOWIEDZIALNOŚCIĄ";

        if (name.Contains(companySuffix))
        {
            name = name.Replace(companySuffix, "").Trim();
        }

        return new UserBasicInfo
        {
            Id = user.Id,
            Email = user.Email,
            Nip = user.NIP,
            Name = name,
            City = user.City,
            Role = user.Role.ToString(),
            IsVerified = user.IsVerified,
            ImageTokensLeft = user.GetImageTokensLeft(),
            TextTokensLeft = user.GetTextTokensLeft(),
            FakturowniaClientId = user.FakturowniaClientId,
            HasSubscription = user.Subscription.StipeSubscriptionId != null,
            FirstPostsGenerated = !user.Privilege.CanGenerateInitialPosts,
        };
    }
}
