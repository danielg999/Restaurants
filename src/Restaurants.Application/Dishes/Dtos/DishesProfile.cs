using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;
using Restaurants.Application.Dishes.Commands.UpdateDishInRestaurant;
using Restaurants.Domain.Entities;
namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishForRestaurantCommand, Dish>();
        CreateMap<UpdateDishInRestaurantCommand, Dish>();
    }
}
