namespace Planner.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public UserDTO(string name, string surname, string email) 
        { 
            Name = name;
            Surname = surname;
            Email = email;
        }
    }
}
