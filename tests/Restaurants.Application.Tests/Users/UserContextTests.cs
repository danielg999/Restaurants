using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var dateOfBirth = new DateOnly(1999, 9, 17);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, "1"),
            new (ClaimTypes.Email, "test@test.com"),
            new (ClaimTypes.Role, "Admin"),
            new (ClaimTypes.Role, "User"),
            new ("Nationality", "Polish"),
            new ("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        // act
        var result = userContext.GetCurrentUser();

        // assert
        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Email.Should().Be("test@test.com");
        result.Roles.Should().ContainInOrder("Admin", "User");
        result.Nationality.Should().Be("Polish");
        result.DateofBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNoPresent_ThrowsInvalidOperationException()
    {
        // arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act
        Action action = () => userContext.GetCurrentUser();

        // assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}