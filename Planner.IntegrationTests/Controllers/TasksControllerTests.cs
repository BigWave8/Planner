using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.DTOs;
using Planner.Models;
using System.Net;
using System.Net.Http.Json;
using Task = System.Threading.Tasks.Task;

namespace Planner.IntegrationTests.Controllers
{
    public class TasksControllerTests
    {
        private PlannerDBContext _context;
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private const string RequestUr = "/api/tasks/";
        private const string ValidName = "test";
        private const string ValidDescription = "test";
        private const Status ValidStatus = Status.ToDo;
        private readonly DateTime ValidCreated = DateTime.Now.AddDays(-1);
        private readonly DateTime ValidDeadline = DateTime.Now.AddDays(1);
        private readonly Guid ValidToDoListId = new("11111111-1111-1111-1111-111111111111");

        [SetUp]
        public void Setup()
        {
            _context = CreateDbContext();
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Create_ValidTask_ReturnOk()
        {
            MockInMemoryDB();
            TaskDTO taskDTO = CreateValidTaskDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, taskDTO);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CleanInMemoryDB();
        }

        [Test]
        public async Task Create_NotValidTask_ReturnBadRequest()
        {
            TaskDTO taskDTO = CreateNotValidTaskDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, taskDTO);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private TaskDTO CreateValidTaskDTO()
            => new(ValidName, ValidDescription, ValidStatus, ValidCreated, ValidDeadline, ValidToDoListId);

        private static TaskDTO CreateNotValidTaskDTO()
            => new("", "", Status.Archived, DateTime.Now, DateTime.Now, Guid.NewGuid());

        private void MockInMemoryDB()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Rostyk",
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com"
                }
            };

            var toDoList = new List<ToDoList>
            {
                new ToDoList
                {
                    Id = ValidToDoListId,
                    Name = ValidName,
                    UserId = new Guid("11111111-1111-1111-1111-111111111111"),
                }
            };

            _context.Users.AddRange(users);
            _context.ToDoLists.AddRange(toDoList);
            _context.SaveChanges();
        }

        private void CleanInMemoryDB()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }

        private static PlannerDBContext CreateDbContext()
            => new PlannerDBContext(
                new DbContextOptionsBuilder<PlannerDBContext>()
                    .UseInMemoryDatabase(databaseName: "MockPlannerDb")
                    .Options);
    }
}
