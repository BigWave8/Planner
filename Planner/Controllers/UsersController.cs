using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services.Interfaces;

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
        public ActionResult<User> CreateUser(UserDTO userDto)
        {
            try
            {
                Guid id = _userService.CreateUser(userDto);
                return CreatedAtAction("GetUserId", new { id }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
