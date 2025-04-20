using Xunit;
using AutoMapper;
using Restaurants.Domain.Entities;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;
    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange
        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test",
            Description = "You eat as much as you can",
            Category = "Indian",
            HasDelivery = true,
            ContactEmail = "test.kebab@x.x",
            ContactNumber = "+48123456788",
            Address = new Address()
            {
                City = "Poznań",
                PostalCode = "12-345",
                Street = "Wrocławska 12",
            },
        };

        // act
        var result = _mapper.Map<RestaurantDto>(restaurant);

        // assert
        result.Should().NotBeNull();
        result.Id.Should().Be(restaurant.Id);
        result.Name.Should().Be(restaurant.Name);
        result.Description.Should().Be(restaurant.Description);
        result.Category.Should().Be(restaurant.Category);
        result.HasDelivery.Should().Be(restaurant.HasDelivery);
        result.City.Should().Be(restaurant.Address.City);
        result.PostalCode.Should().Be(restaurant.Address.PostalCode);
        result.Street.Should().Be(restaurant.Address.Street);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Description = "You eat as much as you can",
            Category = "Indian",
            HasDelivery = true,
            ContactEmail = "test.kebab@x.x",
            ContactNumber = "+48123456788",
            City = "Poznań",
            PostalCode = "12-345",
            Street = "Wrocławska 12",
        };

        // act
        var result = _mapper.Map<Restaurant>(command);

        // assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Category.Should().Be(command.Category);
        result.HasDelivery.Should().Be(command.HasDelivery);
        result.ContactEmail.Should().Be(command.ContactEmail);
        result.ContactNumber.Should().Be(command.ContactNumber);
        result.Address!.City.Should().Be(command.City);
        result.Address!.PostalCode.Should().Be(command.PostalCode);
        result.Address!.Street.Should().Be(command.Street);
    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Test",
            Description = "You eat as much as you can",
            HasDelivery = true,
        };

        // act
        var result = _mapper.Map<Restaurant>(command);

        // assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.HasDelivery.Should().Be(command.HasDelivery);
    }
}