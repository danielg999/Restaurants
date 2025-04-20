using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQueryHandler(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, IMapper mapper, 
    ILogger<GetDishesForRestaurantQueryHandler> logger) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all dishes for restaurantId : {RestaurantId}", request.RestaurantId);
        var restarant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restarant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        var dishes = await dishesRepository.GetAllAsync(restarant.Id);
        var dishesDtos = mapper.Map<IEnumerable<DishDto>>(dishes);
        return dishesDtos;
    }
}
