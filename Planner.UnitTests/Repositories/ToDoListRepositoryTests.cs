using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Planner.Data;
using Planner.Models;
using Planner.Repositories;
using Planner.Repositories.Interfaces;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.Repositories
{
    [TestFixture]
    public class ToDoListRepositoryTests
    {
        private PlannerDBContext _context;
        private IToDoListRepository _repository;
        private const string ValidName = "test";
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");

        [SetUp]
        public void CreateContext()
        {
            _context = CreateDbContext();
            _repository = new ToDoListRepository(_context);
        }

        [Test]
        public void Create_ValidToDoList_ExistInDb()
        {
            ToDoList toDoList = CreateValidToDoList();

            Guid toDoListId = _repository.Create(toDoList);

            _context.ToDoLists.Find(toDoListId).Should().Be(toDoList);
        }

        [TestCase("11111111-1111-1111-1111-111111111111")]
        [TestCase("22222222-2222-2222-2222-222222222222")]
        public void GetById_ToDoListExist_NotNullToDoList(string toDoListId)
        {
            var expected = new ToDoList
            {
                Id = new(toDoListId)
            };

            _context.ToDoLists.Add(expected);
            _context.SaveChanges();

            var actual = _repository.GetById(new(toDoListId));

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetById_ToDoListNotExist_Null()
        {
            var toDoList = new ToDoList
            {
                Id = Guid.NewGuid()
            };

            _context.ToDoLists.Add(toDoList);
            _context.SaveChanges();

            var actual = _repository.GetById(Guid.NewGuid());

            actual.Should().BeNull();
        }

        private static ToDoList CreateValidToDoList()
            => new()
            {
                Id = Guid.NewGuid(),
                Name = ValidName,
                UserId = new("11111111-1111-1111-1111-111111111111"),
            };

        private static PlannerDBContext CreateDbContext()
            => new PlannerDBContext(
                new DbContextOptionsBuilder<PlannerDBContext>()
                    .UseInMemoryDatabase(databaseName: "MockPlannerDb")
                    .Options);
    }
}
