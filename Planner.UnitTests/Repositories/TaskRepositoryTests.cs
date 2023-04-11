using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Models;
using Planner.Repositories;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.Repositories
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        private PlannerDBContext _context;
        private TaskRepository _repository;
        private const string ValidName = "test";
        private const string ValidDescription = "test";
        private const Status ValidStatus = Status.ToDo;
        private readonly DateTime ValidCreated = DateTime.Now.AddDays(-1);
        private readonly DateTime ValidDeadline = DateTime.Now.AddDays(1);
        private readonly Guid ValidToDoListId = new("22222222-2222-2222-2222-222222222222");

        [SetUp]
        public void CreateContext()
        {
            _context = CreateDbContext();
            _repository = new TaskRepository(_context);
        }

        [Test]
        public void Create_ValidTask_ExistInDb()
        {
            Task task = CreateValidTask();

            Guid taskId = _repository.Create(task);

            _context.Tasks.Find(taskId).Should().Be(task);
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

        private static PlannerDBContext CreateDbContext()
            => new PlannerDBContext(
                new DbContextOptionsBuilder<PlannerDBContext>()
                    .UseInMemoryDatabase(databaseName: "MockPlannerDb")
                    .Options);
    }
}
