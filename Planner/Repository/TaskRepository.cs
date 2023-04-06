using Planner.Data;
using Planner.Repository.Interfaces;
using Task = Planner.Models.Task;

namespace Planner.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly PlannerContext _context;
        public TaskRepository(PlannerContext context) 
        { 
            _context = context;
        }
        public Guid Create(Task task)
        {
            _context.Set<Task>().Add(task);
            _context.SaveChanges();
            return task.Id;
        }
    }
}
