using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.DTOs;
using Planner.Models;
using Planner.Services;
using Planner.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

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
        public ActionResult<Task> Create(TaskDTO taskDTO)
        {
            try
            {
                Guid id = _taskService.Create(taskDTO);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
