using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.UpdateDishInRestaurant;

public class UpdateDishInRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, IDishesRepository dishesRepository, IMapper mapper,
    ILogger<UpdateDishInRestaurantCommandHandler> logger) : IRequestHandler<UpdateDishInRestaurantCommand>
{
    public async Task Handle(UpdateDishInRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating dish: {@Dish} for restaurant id: {restaurantId}", request, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dish = await dishesRepository.GetByIdAsync(request.RestaurantId, request.DishId);
        if (dish == null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }
        mapper.Map(request, dish);
        await dishesRepository.SaveChangesAsync();
    }
}
