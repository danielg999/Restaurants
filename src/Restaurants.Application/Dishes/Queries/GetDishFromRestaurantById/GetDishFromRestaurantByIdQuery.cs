using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishFromRestaurantById;

public class GetDishFromRestaurantByIdQuery : IRequest<DishDto>
{
    public GetDishFromRestaurantByIdQuery(int restaurantId, int dishId)
    {
        RestaurantId = restaurantId;
        DishId = dishId;
    }
    public int RestaurantId { get; }
    public int DishId { get; }
}
