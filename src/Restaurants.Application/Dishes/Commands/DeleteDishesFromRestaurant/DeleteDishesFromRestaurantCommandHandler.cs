using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesFromRestaurant;

public class DeleteDishesFromRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, 
    IDishesRepository dishesRepository, ILogger<DeleteDishesFromRestaurantCommandHandler> logger, 
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesFromRestaurantCommand>
{
    public async Task Handle(DeleteDishesFromRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dishes from restaurant with id: {restaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await dishesRepository.DeleteAllAsync(restaurant.Dishes);
    }
}
