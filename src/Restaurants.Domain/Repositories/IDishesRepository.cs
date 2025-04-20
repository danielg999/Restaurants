using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(int restaurantId);
    Task<Dish?> GetByIdAsync(int restaurantId, int dishId);
    Task<int> CreateAsync(Dish entity);
    Task DeleteAsync(Dish entity);
    Task SaveChangesAsync();
    Task DeleteAllAsync(IEnumerable<Dish> dishes);
}
