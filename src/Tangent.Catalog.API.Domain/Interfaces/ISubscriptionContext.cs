using Tangent.Catalog.API.Domain.Models;

namespace Tangent.Catalog.API.Domain.Interfaces;

public interface ISubscriptionContext
{
    DefaultUser GetCurrentSubscriber();

    string GenerateJwtToken(DefaultUser user);
}
