using FluentAssertions;
using Planner.DTOs;
using System.Net;
using System.Net.Http.Json;
using Task = System.Threading.Tasks.Task;

namespace Planner.IntegrationTests.Controllers
{
    public class UsersControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private const string RequestUr = "/api/users/";
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "ro@gmail.com";

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Create_ValidUser_ReturnOk()
        {
            UserDTO userDTO = CreateValidUserDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, userDTO);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Create_NotValidUser_ReturnBadRequest()
        {
            UserDTO userDTO = CreateNotValidUserDTO();

            var response = await _client.PostAsJsonAsync(RequestUr, userDTO);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static UserDTO CreateValidUserDTO()
            => new (ValidName, ValidSurname, ValidEmail);

        private static UserDTO CreateNotValidUserDTO()
            => new("", "", "");
    }
}
