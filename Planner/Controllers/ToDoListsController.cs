using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

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
        public ActionResult<ToDoList> CreateToDoList(ToDoListDTO toDoListDTO)
        {
            try
            {
                Guid id = _toDoListService.CreateToDoList(toDoListDTO);
                return CreatedAtAction("GetToDoListId", new { id }, toDoListDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
