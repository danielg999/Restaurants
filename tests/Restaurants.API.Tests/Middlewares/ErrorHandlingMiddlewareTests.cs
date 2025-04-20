using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegete()
    {
        // arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var nextDelegeteMock = new Mock<RequestDelegate>();

        // act
        await middleware.InvokeAsync(context, nextDelegeteMock.Object);

        // assert
        nextDelegeteMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        // arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // act
        await middleware.InvokeAsync(context, _ => throw notFoundException);

        // assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        // arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var forbidException = new ForbidException();

        // act
        await middleware.InvokeAsync(context, _ => throw forbidException);

        // assert
        context.Response.StatusCode.Should().Be(403);
    }

    [Fact()]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        // arrange
        var context = new DefaultHttpContext();
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var exception = new Exception();

        // act
        await middleware.InvokeAsync(context, _ => throw exception);

        // assert
        context.Response.StatusCode.Should().Be(500);
    }
}
