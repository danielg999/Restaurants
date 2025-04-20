using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishFromRestaurant;

public class DeleteDishFromRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, 
    IDishesRepository dishesRepository, ILogger<CreateDishForRestaurantCommandHandler> logger, 
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishFromRestaurantCommand>
{
    public async Task Handle(DeleteDishFromRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish with id: {dishId} from restaurant with id: {restaurantId}", 
            request.DishId, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        var dish = await dishesRepository.GetByIdAsync(request.RestaurantId, request.DishId);
        if (dish == null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }
        await dishesRepository.DeleteAsync(dish);
    }
}
