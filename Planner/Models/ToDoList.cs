using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class ToDoList : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
