using FluentValidation;
using FluentValidation.Results;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services.Interfaces;
using Task = Planner.Models.Task;

namespace Planner.Services
{
    internal class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IValidator<TaskDTO> _validator;

        public TaskService (ITaskRepository taskRepository, IToDoListRepository toDoListRepository, IValidator<TaskDTO> validator)
        {
            _taskRepository = taskRepository;
            _toDoListRepository = toDoListRepository;
            _validator = validator;
        }

        public Guid Create(TaskDTO taskDTO)
        {
            ValidationResult validationResult = _validator.Validate(taskDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            ToDoList toDoList = _toDoListRepository.GetById(taskDTO.ToDoListId);
            if (toDoList == null)
            {
                throw new Exception("ToDoList not found");
            }

            Task task = Convert(taskDTO);

            return _taskRepository.Create(task);
        }

        private static Task Convert(TaskDTO taskDTO)
        => new Task
        {
            Id = Guid.NewGuid(),
            Name = taskDTO.Name,
            Description = taskDTO.Description,
            Status = taskDTO.Status,
            Created = taskDTO.Created,
            Deadline = taskDTO.Deadline,
            ToDoListId = taskDTO.ToDoListId,
        };
    }
}
