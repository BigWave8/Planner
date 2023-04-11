using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services;
using ValidationException = FluentValidation.ValidationException;

namespace Planner.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "test@gmail.com";
        private const string ExistEmail = "ro@gmail.com";
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IValidator<UserDTO>> validatorMock;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            validatorMock = new Mock<IValidator<UserDTO>>(MockBehavior.Strict);
            userService = new UserService(userRepositoryMock.Object, validatorMock.Object);
            SetupValidatorMock();
        }


        [Test]
        public void CreateUser_WithValidData_ReturnNewGuid()
        {
            var userDto = new UserDTO(ValidName, ValidSurname, ValidEmail);
            SetupUserRepositoryMockCreate();
            SetupUserRepositoryMockCheckIfEmailExist(ValidEmail, false);

            var userId = userService.Create(userDto);

            userId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateUser_WithExistEmail_ThrowValidationException()
        {
            var userDto = new UserDTO(ValidName, ValidSurname, ExistEmail);
            SetupUserRepositoryMockCheckIfEmailExist(ExistEmail, true);

            Action act = () => userService.Create(userDto);

            act.Should().Throw<ValidationException>().Where(e => e.Message.Contains(ExistEmail));
        }

        private void SetupUserRepositoryMockCreate()
            => userRepositoryMock.Setup(r => r.Create(It.IsAny<User>())).Returns(Guid.NewGuid);

        private void SetupUserRepositoryMockCheckIfEmailExist(string email, bool result)
            => userRepositoryMock.Setup(r => r.CheckIfEmailExist(email)).Returns(result);

        private void SetupValidatorMock()
            => validatorMock.Setup(v => v.Validate(It.IsAny<UserDTO>())).Returns(new ValidationResult());
    }
}
