using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Models;
using Planner.Repository;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.RepositoryTests
{
    [TestFixture]
    public class ToDoListRepositoryTests
    {
        public PlannerContext _context;
        private const string ValidName = "test";
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");

        [SetUp]
        public void CreateContext()
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
                    ToDoList = new List<ToDoList>
                    {
                        new ToDoList
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = ValidName,
                            UserId = ValidUserId,
                            Tasks = new List<Models.Task>()
                        },
                        new ToDoList
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = ValidName,
                            UserId = ValidUserId,
                            Tasks = new List<Models.Task>()
                        }
                    }
                },
                new User
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Andrew",
                    Surname = "Scherbyna",
                    Email = "as@gmail.com",
                    ToDoList = new List<ToDoList>()
                }
            };

            _context.User.AddRange(users);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }


        [Test]
        public void Create_ValidToDoList_ExistInDb()
        {
            var repository = new ToDoListRepository(_context);
            var toDoList = CreateValidToDoList();

            var toDoListId = repository.Create(toDoList);

            _context.Set<ToDoList>().Find(toDoListId).Should().Be(toDoList);
        }

        [TestCase("11111111-1111-1111-1111-111111111111")]
        [TestCase("22222222-2222-2222-2222-222222222222")]
        public void GetById_ToDoListExist_NotNullToDoList(string toDoListId)
        {
            var repository = new ToDoListRepository(_context);

            var toDoList = repository.GetById(new(toDoListId));

            toDoList.Should().NotBeNull();
        }

        [Test]
        public void GetById_ToDoListNotExist_Null()
        {
            var repository = new ToDoListRepository(_context);

            var toDoList = repository.GetById(Guid.NewGuid());

            toDoList.Should().BeNull();
        }

        private ToDoList CreateValidToDoList() => new()
        {
            Id = Guid.NewGuid(),
            Name = ValidName,
            UserId = ValidUserId,
            Tasks = new List<Task>()
        };
    }
}
