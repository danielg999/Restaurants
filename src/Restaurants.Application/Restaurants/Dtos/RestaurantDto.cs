using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? LogoSasUrl { get; set; }
    public List<DishDto> Dishes { get; set; } = []; // in .net 8 you can use [] instead of new(); works in the same way

    public static RestaurantDto? FromEntity(Restaurant? restaurant)
    {
        if (restaurant == null) return null;
        return new RestaurantDto
        {
            Name = restaurant.Name,
            Description = restaurant.Description,
            Category = restaurant.Category,
            HasDelivery = restaurant.HasDelivery,
            City = restaurant.Address?.City,
            PostalCode = restaurant.Address?.PostalCode,
            Street = restaurant.Address?.Street,
            Dishes = restaurant.Dishes.Select(DishDto.FromEntity).ToList(),
        };
    }
}
