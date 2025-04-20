using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        handler = new UpdateRestaurantCommandHandler(_restaurantsRepositoryMock.Object,
            _loggerMock.Object, _mapperMock.Object, _restaurantAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId, // fyi only id is needed here for this test
            Name = "name test",
            Description = "description test",
            HasDelivery = true,
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId, // fyi only id is needed here for this test
            Name = "test",
            Description = "test",
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);
        
        // act
        await handler.Handle(command, CancellationToken.None);

        // assert
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
        _restaurantsRepositoryMock.Verify(m => m.SaveChangesAsync(), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange
        var restaurantId = 2;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact()]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // arrange
        var restaurantId = 3;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(m => m.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);
        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        // act
        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

        // assert
        await action.Should().ThrowAsync<ForbidException>();
    }
}
