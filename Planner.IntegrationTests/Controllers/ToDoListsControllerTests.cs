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
    public class ToDoListsControllerTests
    {
        private PlannerDBContext _context;
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private const string RequestUr = "/api/toDoLists/";
        private const string ValidName = "test";
        private readonly Guid ValidUserId = new("11111111-1111-1111-1111-111111111111");

        [SetUp]
        public void Setup()
        {
            _context = CreateDbContext();
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Create_ValidToDoList_ReturnOk()
        {
            MockInMemoryDB();
            ToDoListDTO toDoListDTO = CreateValidToDoListDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, toDoListDTO);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CleanInMemoryDB();
        }

        [Test]
        public async Task Create_NotValidToDoList_ReturnBadRequest()
        {
            ToDoListDTO toDoListDTO = CreateNotValidToDoListDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, toDoListDTO);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private ToDoListDTO CreateValidToDoListDTO()
            => new (ValidName, ValidUserId);

        private static ToDoListDTO CreateNotValidToDoListDTO()
            => new ("", Guid.NewGuid());

        private void MockInMemoryDB()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = ValidUserId,
                    Name = "Rostyk",
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com"
                }
            };

            _context.Users.AddRange(users);
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
