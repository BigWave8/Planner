using Planner.DTOs;
using Planner.Models;

namespace Planner.Services.Interfaces
{
    public interface IUserService
    {
        Guid Create(UserDTO userDto);
        User GetById(Guid id);
    }
}
