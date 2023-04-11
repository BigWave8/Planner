using FluentValidation;
using FluentValidation.Results;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services.Interfaces;
using Planner.Validators;
using Task = Planner.Models.Task;

namespace Planner.Services
{
    internal class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<ToDoListDTO> _validator;

        public ToDoListService(IToDoListRepository toDoListRepository, IUserRepository userRepository, IValidator<ToDoListDTO> validator) 
        {
            _toDoListRepository = toDoListRepository;
            _userRepository = userRepository;
            _validator = validator;
        }
        public Guid Create(ToDoListDTO toDoListDTO)
        {
            ValidationResult validationResult = _validator.Validate(toDoListDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            User user = _userRepository.GetById(toDoListDTO.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            ToDoList toDoList = Convert(toDoListDTO);

            return _toDoListRepository.Create(toDoList);
        }

        public ToDoList GetById(Guid id)
            => _toDoListRepository.GetById(id);

        private static ToDoList Convert(ToDoListDTO toDoListDTO)
        => new ToDoList
        {
            Id = Guid.NewGuid(),
            Name = toDoListDTO.Name,
            UserId = toDoListDTO.UserId,
        };
    }
}
