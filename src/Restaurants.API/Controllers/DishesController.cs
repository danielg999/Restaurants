using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;
using Restaurants.Application.Dishes.Commands.DeleteDishesFromRestaurant;
using Restaurants.Application.Dishes.Commands.DeleteDishFromRestaurant;
using Restaurants.Application.Dishes.Commands.UpdateDishInRestaurant;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishFromRestaurantById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("/api/restaurants/{restaurantId}/dishes")]
[Authorize]
public class DishesController(IMediator mediator) : ControllerBase
{
    
    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeast20)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll([FromRoute]int restaurantId)
    {
        var restaurants = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(restaurants);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto>> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var restaurant = await mediator.Send(new GetDishFromRestaurantByIdQuery(restaurantId, dishId));
        return Ok(restaurant);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishForRestaurantCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { restaurantId, dishId }, null);
    }

    [HttpPatch("{dishId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDish([FromRoute] int restaurantId, [FromRoute] int dishId, [FromBody] UpdateDishInRestaurantCommand command)
    {
        command.DishId = dishId;
        command.RestaurantId = restaurantId;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{dishId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        await mediator.Send(new DeleteDishFromRestaurantCommand(restaurantId, dishId));
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDishes([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteDishesFromRestaurantCommand(restaurantId));
        return NoContent();
    }
}
