using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services;
using Planner.Services.Interfaces;
using Task = Planner.Models.Task;

namespace Planner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public ActionResult<Task> CreateTask(TaskDTO taskDTO)
        {
            try
            {
                Guid id = _taskService.CreateTask(taskDTO);
                return CreatedAtAction("GetTaskId", new { id }, taskDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
