using FluentAssertions;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services;

namespace Planner.UnitTests.ServicesTests
{
    [TestFixture]
    public class ToDoListServiceTests
    {
        private const string ValidName = "test";
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");
        private const string ExistName = "test";
        private const string ExistSurname = "test";
        private const string ExistEmail = "ro@gmail.com";
        private readonly Guid NotExistUserId = Guid.NewGuid();
        private Mock<IToDoListRepository> toDoListRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private ToDoListService toDoListService;

        [SetUp]
        public void Setup()
        {
            toDoListRepositoryMock = new Mock<IToDoListRepository>(MockBehavior.Strict);
            userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            toDoListService = new ToDoListService(toDoListRepositoryMock.Object, userRepositoryMock.Object);
        }


        [Test]
        public void CreateToDoList_WithValidData_ReturnNewGuid()
        {
            var toDoListDTO = new ToDoListDTO(ValidName, ValidUserId);
            SetupRepositoryMockToGetValidUserAndCorrectCreateToDoList();

            var toDoListId = toDoListService.CreateToDoList(toDoListDTO);

            toDoListId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateToDoList_WithNotExistUser_ThrowUserNotFoundException()
        {
            var toDoListDTO = new ToDoListDTO(ValidName, NotExistUserId);
            SetupRepositoryMockToTryGetNotExistUser();

            Action act = () => toDoListService.CreateToDoList(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("User not found"));
        }

        private User ValidUser() => new()
        {
            Id = ValidUserId,
            Name = ExistName,
            Surname = ExistSurname,
            Email = ExistEmail
        };

        private void SetupRepositoryMockToGetValidUserAndCorrectCreateToDoList()
        {
            userRepositoryMock.Setup(r => r.GetById(ValidUserId)).Returns(ValidUser()); //
            toDoListRepositoryMock.Setup(r => r.Create(It.IsAny<ToDoList>())).Returns(Guid.NewGuid());
        }

        private void SetupRepositoryMockToTryGetNotExistUser()
        {
            userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<User>(null);
        }
    }
}
