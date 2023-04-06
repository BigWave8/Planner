using Planner.DTOs;
using Planner.Models;

namespace Planner.Services.Interfaces
{
    public interface IUserService
    {
        Guid CreateUser(UserDTO userDto);
        User GetUserById(Guid id);
    }
}
