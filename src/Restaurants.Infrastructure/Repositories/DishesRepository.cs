using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<IEnumerable<Dish>> GetAllAsync(int restaurantId)
    {
        var dishes = await dbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToListAsync();
        return dishes;
    }

    public async Task<Dish?> GetByIdAsync(int restaurantId, int dishId)
    {
        var dish = await dbContext.Dishes.Where(d => d.RestaurantId == restaurantId && d.Id == dishId).FirstOrDefaultAsync();
        return dish;
    }

    public async Task<int> CreateAsync(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(Dish entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();

    public async Task DeleteAllAsync(IEnumerable<Dish> dishes)
    {
        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }
}
