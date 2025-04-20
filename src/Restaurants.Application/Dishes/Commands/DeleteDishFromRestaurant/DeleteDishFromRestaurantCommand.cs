using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishFromRestaurant;

public class DeleteDishFromRestaurantCommand : IRequest
{
    public DeleteDishFromRestaurantCommand(int restaurantId, int dishId)
    {
        RestaurantId = restaurantId;
        DishId = dishId;
    }
    public int RestaurantId { get; }
    public int DishId { get; }
}
