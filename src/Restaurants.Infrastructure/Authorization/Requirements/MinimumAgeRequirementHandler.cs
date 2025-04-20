using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User email: {UserEmail}, date of birth: {DoB} - Handling MinimumAgeRequirement",
            currentUser!.Email, currentUser.DateofBirth);

        if (currentUser.DateofBirth == null)
        {
            logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateofBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation("Authorization failed");
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
