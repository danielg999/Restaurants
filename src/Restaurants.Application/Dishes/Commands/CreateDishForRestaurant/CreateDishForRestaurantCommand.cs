﻿using MediatR;

namespace Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;

public class CreateDishForRestaurantCommand : IRequest<int>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public int RestaurantId { get; set; }
}
