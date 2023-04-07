using Planner.Models;

namespace Planner.DTOs
{
    public class TaskDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid ToDoListId { get; set; }

        public TaskDTO(string name, string? description, Status status, DateTime created, DateTime? deadline, Guid toDoListId) 
        { 
            Name = name;
            Description = description;
            Status = status;
            Created = created;
            Deadline = deadline;
            ToDoListId = toDoListId;
        }
    }
}
