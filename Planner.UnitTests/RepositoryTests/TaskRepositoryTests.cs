using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Models;
using Planner.Repository;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.RepositoryTests
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        private PlannerContext _context;
        private TaskRepository _repository;
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");
        private const string ValidName = "test";
        private const string ValidDescription = "test";
        private const Status ValidStatus = Status.ToDo;
        private readonly DateTime ValidCreated = DateTime.Now.AddDays(-1);
        private readonly DateTime ValidDeadline = DateTime.Now.AddDays(1);
        private readonly Guid ValidToDoListId = new("22222222-2222-2222-2222-222222222222");

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
                    Id = ValidUserId,
                    Name = ValidName,
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com",
                    ToDoList = new List<ToDoList>
                    {
                        new ToDoList
                        {
                            Id = ValidToDoListId,
                            Name = ValidName,
                            UserId = ValidUserId,
                            Tasks = new List<Task>()
                        }
                    }
                },
            };

            _context.User.AddRange(users);
            _context.SaveChanges();

            _repository = new TaskRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }


        [Test]
        public void Create_ValidTask_ExistInDb()
        {
            Task task = CreateValidTask();

            Guid taskId = _repository.Create(task);

            _context.Set<Task>().Find(taskId).Should().Be(task);
        }

        private Task CreateValidTask()
        {
            return new Task
            {
                Id = Guid.NewGuid(),
                Name = ValidName,
                Description = ValidDescription,
                Status = ValidStatus,
                Created = ValidCreated,
                Deadline = ValidDeadline,
                ToDoListId = ValidToDoListId
            };
        }
    }
}
