using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;

public class CreateDishForRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, IDishesRepository dishesRepository, IMapper mapper, 
    ILogger<CreateDishForRestaurantCommandHandler> logger) : IRequestHandler<CreateDishForRestaurantCommand, int>
{
    public async Task<int> Handle(CreateDishForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating dish: {@Dish} for restaurantId: {restaurantId}", request, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dish = mapper.Map<Dish>(request);
        var dishId = await dishesRepository.CreateAsync(dish);

        return dishId;
    }
}
