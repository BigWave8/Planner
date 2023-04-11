namespace Planner.DTOs
{
    public class ToDoListDTO
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public ToDoListDTO(string name, Guid userId) 
        {
            Name = name;
            UserId = userId;
        }
    }
}
