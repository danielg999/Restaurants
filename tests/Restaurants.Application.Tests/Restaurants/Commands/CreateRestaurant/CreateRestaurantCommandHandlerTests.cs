using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // arrange
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(m => m.CreateAsync(It.IsAny<Restaurant>())).ReturnsAsync(1);

        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Restaurant>(It.IsAny<CreateRestaurantCommand>())).Returns(restaurant);

        var userContext = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner_id", "test@test.com", [], null, null);
        userContext.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var handler = new CreateRestaurantCommandHandler(restaurantsRepositoryMock.Object,
            loggerMock.Object, mapperMock.Object, userContext.Object);

        // act
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner_id");
        restaurantsRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once());
    }
}