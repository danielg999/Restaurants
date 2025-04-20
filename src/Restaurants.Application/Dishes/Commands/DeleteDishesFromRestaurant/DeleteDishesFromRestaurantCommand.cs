using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishesFromRestaurant;

public class DeleteDishesFromRestaurantCommand : IRequest
{
    public DeleteDishesFromRestaurantCommand(int restaurantId)
    {
        RestaurantId = restaurantId;
    }

    public int RestaurantId { get; }
}
