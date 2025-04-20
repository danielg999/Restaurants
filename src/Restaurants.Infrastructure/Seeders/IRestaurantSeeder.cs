using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Seeders
{
    public interface IRestaurantSeeder
    {
        public Task Seed();
    }
}