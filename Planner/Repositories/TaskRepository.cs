using Planner.Data;
using Planner.Repositories.Interfaces;
using Task = Planner.Models.Task;

namespace Planner.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private readonly PlannerDBContext _context;
        public TaskRepository(PlannerDBContext context) 
        { 
            _context = context;
        }
        public Guid Create(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task.Id;
        }
    }
}
