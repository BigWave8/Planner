using FluentValidation;
using Planner.DTOs;

namespace Planner.Validators
{
    internal class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can`t be empty")
                .Length(2, 30).WithMessage("Name must be between 2 and 30 characters");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname can`t be empty")
                .Length(2, 30).WithMessage("Surname must be between 2 and 30 characters")
                .Matches("^[a-zA-Z]+$").WithMessage("Surname must contain only letters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can`t be empty")
                .EmailAddress().WithMessage("It's not like email");
        }
    }
}
