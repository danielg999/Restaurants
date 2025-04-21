using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Domain.Repositories;
using Moq;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.API.Tests;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvalutaor>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                        _ => _restaurantsRepositoryMock.Object));

                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder),
                        _ => _restaurantSeederMock.Object));
                });
            });
    }
    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact()]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async Task GetById_ForExistingId_ShouldReturn200Ok()
    {
        // arrange
        var restaurantId = 999;
        var client = _factory.CreateClient();
        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "test",
            Description = "test description"
        };
        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        // act
        var result = await client.GetAsync($"/api/restaurants/{restaurantId}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);

    }

    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // arrange
        var restaurantId = 999;
        var client = _factory.CreateClient();

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // act
        var result = await client.GetAsync($"/api/restaurants/{restaurantId}");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
