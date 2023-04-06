using Microsoft.EntityFrameworkCore;
using Planner.Models;
using Task = Planner.Models.Task;

namespace Planner.Data
{
    public class PlannerContext : DbContext
    {
        public PlannerContext(DbContextOptions<PlannerContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MockPlannerDb");
        }

        public DbSet<User> User { get; set; } = default!;
    }
}
