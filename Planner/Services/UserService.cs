using FluentValidation;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services.Interfaces;
using Planner.Validators;

namespace Planner.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Guid CreateUser(UserDTO userDto)
        {
            UserValidator validator = new();
            validator.ValidateAndThrow(userDto);
            if (_userRepository.CheckIfEmailExist(userDto.Email))
            {
                throw new ValidationException($"User with email {userDto.Email} already exists");
            }

            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Surname = userDto.Surname,
                Email = userDto.Email.ToLower(),
                ToDoList = new List<ToDoList>()
            };
            Guid userId = _userRepository.Create(user);

            return userId;
        }

        public User GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }
    }
}
