using FluentValidation;
namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(r => r.Name)
            .Length(3, 100);

        RuleFor(r => r.Category)
            //.Must(category => validCategories.Contains(category))
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");
        //.Custom((value, context) =>
        //{
        //    var isValidCategory = validCategories.Contains(value);
        //    if (!isValidCategory)
        //    {
        //        context.AddFailure("Category", "Invalid category. Please choose from the valid categories.");
        //    }
        //});

        RuleFor(r => r.ContactNumber)
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
            .WithMessage("Please provide valid a contact number.");

        RuleFor(r => r.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide valid a email address.");

        RuleFor(r => r.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}
