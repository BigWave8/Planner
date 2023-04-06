using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Planner.Data;
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
        private PlannerContext _context;

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
            SetupInMemoryDb();
            var toDoListDTO = new ToDoListDTO(ValidName, ValidUserId);
            SetupRepositoryMockToGetValidUserAndCorrectCreateToDoList();

            var toDoListId = toDoListService.CreateToDoList(toDoListDTO);

            toDoListId.Should().NotBe(Guid.Empty);
            TearDown();
        }

        [Test]
        public void CreateToDoList_WithNotExistUser_ThrowValidationException()
        {
            var toDoListDTO = new ToDoListDTO(ValidName, NotExistUserId);
            SetupRepositoryMockToTryGetNotExistUser();

            Action act = () => toDoListService.CreateToDoList(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("User not found"));
        }

        private void SetupInMemoryDb()
        {
            var builder = new DbContextOptionsBuilder<PlannerContext>();
            builder.UseInMemoryDatabase(databaseName: "MockPlannerDb");

            _context = new PlannerContext(builder.Options);

            var users = new List<User>
            {
                new User
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Rostyk",
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com",
                    ToDoList = new List<ToDoList>()
                }
            };

            _context.User.AddRange(users);
            _context.SaveChanges();
        }

        private void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }

        private User ValidUser() => new()
        {
            Id = ValidUserId,
            Name = ExistName,
            Surname = ExistSurname,
            Email = ExistEmail,
            ToDoList = new List<ToDoList>()
        };

        private void SetupRepositoryMockToGetValidUserAndCorrectCreateToDoList()
        {
            userRepositoryMock.Setup(r => r.GetById(ValidUserId)).Returns(ValidUser());
            toDoListRepositoryMock.Setup(r => r.Create(It.IsAny<ToDoList>())).Returns(Guid.NewGuid());
        }

        private void SetupRepositoryMockToTryGetNotExistUser()
        {
            userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((User)null);
        }
    }
}
