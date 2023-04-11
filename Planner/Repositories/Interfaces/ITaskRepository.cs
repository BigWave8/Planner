using Task = Planner.Models.Task;

namespace Planner.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Guid Create(Task task);
    }
}
