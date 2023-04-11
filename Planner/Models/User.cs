using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class User : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
