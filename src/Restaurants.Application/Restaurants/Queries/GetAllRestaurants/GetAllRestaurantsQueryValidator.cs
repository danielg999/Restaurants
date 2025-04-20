using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowedPageSize = [5, 10, 15, 30];
    private string[] allowedColumnsToSortBy = [
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category),
        ];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(allowedPageSize.Contains)
            .WithMessage($"Page size must be in [{string.Join(",", allowedPageSize)}]");

        RuleFor(r => r.SortBy)
            .Must(allowedColumnsToSortBy.Contains)
            .When(r => r.SortBy != null)
            .WithMessage($"Column to sort by must be in [{string.Join(",", allowedColumnsToSortBy)}]");
    }
}
