using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated) 
    : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; } = minimumRestaurantsCreated;
}
