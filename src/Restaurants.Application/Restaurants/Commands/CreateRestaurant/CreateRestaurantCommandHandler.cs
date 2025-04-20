using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger, IMapper mapper, IUserContext userContext
    //,IRestaurantAuthorizationService restaurantAuthorizationService
    ) 
    : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("{UserEmail} [{UserId}] Creating a new restaurant {@Restaurant}", 
            user!.Email, user.Id, request);
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = user.Id;

        //if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
        //    throw new ForbidException();

        int id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}
