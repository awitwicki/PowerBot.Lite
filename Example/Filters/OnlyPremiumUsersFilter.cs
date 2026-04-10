using PowerBot.Lite.Attributes;

namespace Example.Filters;

public class OnlyPremiumUsersFilter: BaseHandlerFilter
{
    protected override Task<bool> FilterAsync()
    {
        return Task.FromResult(User.IsPremium);
    }
}
