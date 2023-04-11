using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services;

namespace Planner.UnitTests.Services
{
    [TestFixture]
    public class ToDoListServiceTests
    {
        private const string ValidName = "test";
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");
        private const string ExistName = "test";
        private const string ExistSurname = "test";
        private const string ExistEmail = "ro@gmail.com";
        private Mock<IToDoListRepository> toDoListRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IValidator<ToDoListDTO>> validatorMock;
        private ToDoListService toDoListService;

        [SetUp]
        public void Setup()
        {
            toDoListRepositoryMock = new Mock<IToDoListRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            validatorMock = new Mock<IValidator<ToDoListDTO>>(MockBehavior.Strict);
            toDoListService = new ToDoListService(toDoListRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object);
            SetupToDoListRepositoryMock();
            SetupValidatorMock();
        }


        [Test]
        public void CreateToDoList_WithValidData_ReturnNewGuid()
        {
            var toDoListDTO = new ToDoListDTO(ValidName, ValidUserId);
            SetupUserRepositoryMock(ValidUserId, ValidUser());

            var toDoListId = toDoListService.Create(toDoListDTO);

            toDoListId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateToDoList_WithNotExistUser_ThrowUserNotFoundException()
        {
            Guid notExistUserId = Guid.NewGuid();
            var toDoListDTO = new ToDoListDTO(ValidName, notExistUserId);
            SetupUserRepositoryMock(notExistUserId, null);

            Action act = () => toDoListService.Create(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("User not found"));
        }

        private User ValidUser() => new()
        {
            Id = ValidUserId,
            Name = ExistName,
            Surname = ExistSurname,
            Email = ExistEmail
        };

        private void SetupToDoListRepositoryMock()
            => toDoListRepositoryMock.Setup(r => r.Create(It.IsAny<ToDoList>())).Returns(Guid.NewGuid());

        private void SetupUserRepositoryMock(Guid id, User? user)
            => userRepositoryMock.Setup(r => r.GetById(id)).Returns(user);

        private void SetupValidatorMock()
            => validatorMock.Setup(v => v.Validate(It.IsAny<ToDoListDTO>())).Returns(new ValidationResult());
    }
}
