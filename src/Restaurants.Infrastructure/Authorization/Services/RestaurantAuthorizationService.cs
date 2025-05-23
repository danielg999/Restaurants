﻿using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

internal class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, 
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user: {UserEmail} to {Operation} for restaurant: {RestaurantName}",
            user!.Email, resourceOperation, restaurant.Name);

        if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
        {
            logger.LogInformation("Create/read operation - succesfull authorization");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Delete operation - succesfull authorization");
            return true;
        }

        if (resourceOperation == ResourceOperation.Update && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Update operation - succesfull authorization");
            return true;
        }

        return false;
    }
}
