using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveAnyValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Category = "Italian",
            PostalCode = "12-345",
            ContactEmail = "test@test.com",
            ContactNumber = "+48123456789"
        };
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Ita",
            PostalCode = "12345",
            ContactEmail = "@test.com",
            ContactNumber = "123456789"
        };
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        result.ShouldHaveValidationErrorFor(c => c.ContactNumber);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            Category = category,
        };
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory()]
    [InlineData("12345")]
    [InlineData("123-45")]
    [InlineData("12 345")]
    [InlineData("12-3 45")]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrorsForPostalCodeProperty(string postalCode)
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode,
        };
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory()]
    [InlineData("123456789")]
    [InlineData("123 456 789")]
    [InlineData("123-456-789")]
    [InlineData("+123456789")]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrorsForContactNumberProperty(string contactNumber)
    {
        // arrange
        var command = new CreateRestaurantCommand()
        {
            ContactNumber = contactNumber,
        };
        var validator = new CreateRestaurantCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.ContactNumber);
    }
}