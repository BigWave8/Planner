using Planner.DTOs;

namespace Planner.Services.Interfaces
{
    public interface ITaskService
    {
        Guid CreateTask(TaskDTO taskDTO);
    }
}
