using Microsoft.EntityFrameworkCore;
using Planner.Models;
using Task = Planner.Models.Task;

namespace Planner.Data
{
    public class PlannerDBContext : DbContext
    {
        public PlannerDBContext(DbContextOptions<PlannerDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<ToDoList> ToDoLists { get; set; }
    }
}
