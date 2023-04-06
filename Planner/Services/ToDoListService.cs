using FluentValidation;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services.Interfaces;
using Planner.Validators;
using Task = Planner.Models.Task;

namespace Planner.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IUserRepository _userRepository;
        public ToDoListService(IToDoListRepository toDoListRepository, IUserRepository userRepository) 
        {
            _toDoListRepository = toDoListRepository;
            _userRepository = userRepository;
        }
        public Guid CreateToDoList(ToDoListDTO toDoListDTO)
        {
            ToDoListValidator validator = new();
            validator.ValidateAndThrow(toDoListDTO);

            var user = _userRepository.GetById(toDoListDTO.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var toDoList = new ToDoList
            {
                Id = Guid.NewGuid(),
                Name = toDoListDTO.Name,
                UserId = toDoListDTO.UserId,
                Tasks = new List<Task>()
            };
            Guid toDoListId = _toDoListRepository.Create(toDoList);

            return toDoListId;
        }

        public ToDoList GetToDoListById(Guid id)
        {
            return _toDoListRepository.GetById(id);
        }
    }
}
