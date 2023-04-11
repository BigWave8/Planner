using FluentValidation;
using FluentValidation.Results;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services.Interfaces;

namespace Planner.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserDTO> _validator;

        public UserService(IUserRepository userRepository, IValidator<UserDTO> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public Guid Create(UserDTO userDto)
        {
            ValidationResult validationResult = _validator.Validate(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (_userRepository.CheckIfEmailExist(userDto.Email))
            {
                throw new ValidationException($"User with email {userDto.Email} already exists");
            }

            User user = Convert(userDto);

            return _userRepository.Create(user);
        }

        public User GetById(Guid id)
            => _userRepository.GetById(id);

        private static User Convert(UserDTO userDto)
        => new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Surname = userDto.Surname,
            Email = userDto.Email.ToLower()
        };
    }
}
