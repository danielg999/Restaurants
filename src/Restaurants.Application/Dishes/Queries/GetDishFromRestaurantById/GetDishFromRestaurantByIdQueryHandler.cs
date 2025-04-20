using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishFromRestaurantById;

public class GetDishFromRestaurantByIdQueryHandler(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, IMapper mapper, 
    ILogger<GetDishFromRestaurantByIdQueryHandler> logger) : IRequestHandler<GetDishFromRestaurantByIdQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishFromRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish with id: {dishId} for restaurant id: {restaurantId}", request.DishId, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dish = await dishesRepository.GetByIdAsync(restaurant.Id, request.DishId);
        if (dish is null)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }
        var dishDto = mapper.Map<DishDto>(dish);
        return dishDto;
    }
}
