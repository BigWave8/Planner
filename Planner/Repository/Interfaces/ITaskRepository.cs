using Task = Planner.Models.Task;

namespace Planner.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Guid Create(Task task);
    }
}
