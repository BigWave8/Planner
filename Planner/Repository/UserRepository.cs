using Planner.Data;
using Planner.Models;
using Planner.Repository.Interfaces;

namespace Planner.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PlannerContext _context;

        public UserRepository(PlannerContext context)
        {
            _context = context;
        }

        public Guid Create(User user)
        {
            _context.Set<User>().Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public bool CheckIfEmailExist(string email)
        {
            return _context.Set<User>()
                .Any(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public User GetById(Guid id)
        {
            return _context.Set<User>().Find(id);
        }
    }
}
