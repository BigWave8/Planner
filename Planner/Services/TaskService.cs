using FluentValidation;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services.Interfaces;
using Planner.Validators;
using Task = Planner.Models.Task;

namespace Planner.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IToDoListRepository _toDoListRepository;
        public TaskService (ITaskRepository taskRepository, IToDoListRepository toDoListRepository)
        {
            _taskRepository = taskRepository;
            _toDoListRepository = toDoListRepository;
        }

        public Guid CreateTask(TaskDTO taskDTO)
        {
            TaskValidator validator = new();
            validator.ValidateAndThrow(taskDTO);

            ToDoList toDoList = _toDoListRepository.GetById(taskDTO.ToDoListId);
            if (toDoList == null)
            {
                throw new Exception("ToDoList not found");
            }

            Task task = new()
            {
                Id = Guid.NewGuid(),
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Status = taskDTO.Status,
                Created = taskDTO.Created,
                Deadline = taskDTO.Deadline,
                ToDoListId = taskDTO.ToDoListId,
            };
            Guid taskId = _taskRepository.Create(task);
            return taskId;
        }
    }
}
