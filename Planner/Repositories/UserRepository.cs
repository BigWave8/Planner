using Planner.Data;
using Planner.Models;
using Planner.Repositories.Interfaces;

namespace Planner.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly PlannerDBContext _context;

        public UserRepository(PlannerDBContext context)
        {
            _context = context;
        }

        public Guid Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public bool CheckIfEmailExist(string email)
            => _context
                .Users
                .Any(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));

        public User GetById(Guid id)
            => _context.Users.Find(id);
    }
}
