using Planner.Models;

namespace Planner.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Guid Create(User user);
        bool CheckIfEmailExist(string email);
        User GetById(Guid id);
    }
}
