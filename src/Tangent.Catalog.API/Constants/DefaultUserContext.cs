using Tangent.Catalog.API.Domain.Models;

namespace Tangent.Catalog.API.Constants;

/// <summary>
/// replace with user context finding users by their subscriberIds or userIds 
/// </summary>
public static class DefaultUserContext
{
   public static async Task<DefaultUser> GetUser(string subscriberId)
   {
      // Simulate a delay of 1 second
      await Task.Delay(1000);

      return new DefaultUser()
      {
         SubscriberId = subscriberId,
         Name = "Jane Doe",
         Email = "jane@mil.com"
      };
   }
}
