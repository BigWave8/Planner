using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using Task = System.Threading.Tasks.Task;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;

        public ToDoListsController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        [HttpPost]
        public ActionResult<Task> Create(ToDoListDTO toDoListDTO)
        {
            try
            {
                Guid id = _toDoListService.Create(toDoListDTO);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
