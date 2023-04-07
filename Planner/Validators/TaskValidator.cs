using FluentValidation;
using Planner.DTOs;
using Planner.Models;

namespace Planner.Validators
{
    public class TaskValidator : AbstractValidator<TaskDTO>
    {
        public TaskValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can`t be empty")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.Status)
                .Must(BeStatusOnCreate).WithMessage("On create status must be ToDo or InProgress or ForLater");

            RuleFor(x => x.Created)
                .NotEmpty().WithMessage("Created time must be specified")
                .GreaterThan(new DateTime(DateTime.Now.Year - 1, 1, 1)).WithMessage("Task must be created no earlier than last year")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Created time can`t be in the future");

            RuleFor(x => x.Deadline)
                .GreaterThan(x => x.Created).WithMessage("Deadline must be later than created time")
                .LessThanOrEqualTo(x => x.Created.AddYears(1)).WithMessage("Deadline should not exceed one year from created time");

            RuleFor(x => x.ToDoListId)
                .NotEmpty().WithMessage("ToDoList id is required");
        }

        private bool BeStatusOnCreate(Status status) => 
            status == Status.ToDo ||
            status == Status.InProgress ||
            status == Status.ForLater;
    }
}
