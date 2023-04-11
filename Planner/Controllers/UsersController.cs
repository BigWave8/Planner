using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<Task> Create(UserDTO userDto)
        {
            try
            {
                Guid id = _userService.Create(userDto);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
