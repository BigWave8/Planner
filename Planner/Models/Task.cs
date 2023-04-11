using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class Task : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public Status Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Deadline { get; set; }

        public Guid ToDoListId { get; set; }

        public ToDoList ToDoList { get; set; }
    }
}
