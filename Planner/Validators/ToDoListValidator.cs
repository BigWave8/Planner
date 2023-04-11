using FluentValidation;
using Planner.DTOs;

namespace Planner.Validators
{
    internal class ToDoListValidator : AbstractValidator<ToDoListDTO>
    {
        public ToDoListValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can`t be empty")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required");
        }
    }
}
