using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Models.Validators
{
    public class RestaurantQueryValidator:AbstractValidator<RestaurantQuery>
    {
        private int[] allowedNumbers = { 5, 10, 15 };
        private string[] allowedSortBy = { nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description) };
        public RestaurantQueryValidator()
        {
            RuleFor(x => x.pageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.pageSize).Custom((value, context) =>
            {
                if(!allowedNumbers.Contains(value))
                {
                    context.AddFailure("Not allowed page size");
                }
            }
            );
            RuleFor(x => x.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortBy.Contains(value)).WithMessage("SorBY is optional but must be in specific range");

        }
    }
}
