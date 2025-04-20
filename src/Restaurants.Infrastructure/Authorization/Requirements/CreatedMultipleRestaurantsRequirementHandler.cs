using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class CreatedMultipleRestaurantsRequirementHandler(ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
    IUserContext userContext, IRestaurantsRepository restaurantsRepository) 
    : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        var restaurants = await restaurantsRepository.GetAllAsync();
        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);
        logger.LogInformation("User email: {UserEmail}, user restaurants created: {URC} - Handling MinimumAgeRequirement",
            currentUser!.Email, userRestaurantsCreated);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation("Authorization failed");
            context.Fail();
        }
    }
}
