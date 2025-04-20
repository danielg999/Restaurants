using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDishForRestaurant;

public class CreateDishForRestaurantCommandValidator : AbstractValidator<CreateDishForRestaurantCommand>
{
    public CreateDishForRestaurantCommandValidator()
    {
        RuleFor(d => d.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");

        RuleFor(d => d.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Kilo calories must be a non-negative number.");
    }
}
