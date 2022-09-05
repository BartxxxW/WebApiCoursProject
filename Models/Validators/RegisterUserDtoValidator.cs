using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;

namespace WebApplication44Udemy.Models.Validators
{
    public class RegisterUserDtoValidator:AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).Custom((value, context) =>
              {
                  if(dbContext.Users.Any(e=>e.Email==value))
                  {
                      context.AddFailure("Email in Use");
                  }
              });
            RuleFor(p => p.Password).MinimumLength(6);
            RuleFor(p => p.Password).MinimumLength(6);
            RuleFor(p => p.ConfirmPassword).NotEmpty();
            RuleFor(p => p.Password).NotEmpty();
            RuleFor(p => p.Password).Equal(p => p.ConfirmPassword);


        }

    }
}
