using FluentAssertions;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services;
using ValidationException = FluentValidation.ValidationException;

namespace Planner.UnitTests.ServicesTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "test@gmail.com";
        private const string ExistEmail = "ro@gmail.com";
        private Mock<IUserRepository> userRepositoryMock;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userService = new UserService(userRepositoryMock.Object);
        }


        [Test]
        public void CreateUser_WithValidData_ReturnNewGuid()
        {
            var userDto = new UserDTO(ValidName, ValidSurname, ValidEmail);
            SetupRepositoryMockToCorrectCreateUserAndCheckWithValidEmail();
            
            var userId = userService.CreateUser(userDto);

            userId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateUser_WithExistEmail_ThrowValidationException()
        {
            var userDto = new UserDTO(ValidName, ValidSurname, ExistEmail);
            SetupRepositoryMockToCheckWithExistEmail();

            Action act = () => userService.CreateUser(userDto);

            act.Should().Throw<ValidationException>().Where(e => e.Message.Contains(ExistEmail));
        }


        private void SetupRepositoryMockToCorrectCreateUserAndCheckWithValidEmail()
        {
            userRepositoryMock.Setup(r => r.CheckIfEmailExist(ValidEmail)).Returns(false);
            userRepositoryMock.Setup(r => r.Create(It.IsAny<User>())).Returns(Guid.NewGuid);
        }

        private void SetupRepositoryMockToCheckWithExistEmail()
        {
            userRepositoryMock.Setup(r => r.CheckIfEmailExist(ExistEmail)).Returns(true);
        }
    }
}
