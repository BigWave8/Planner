using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.

namespace Planner.IntegrationTests.ControllersTests
{
    public class TasksControllerTests
    {
        [Test]
        public async Task When_ReturnOk()
        {
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            var responce = await client.GetAsync("WeatherForecast");
            responce.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
