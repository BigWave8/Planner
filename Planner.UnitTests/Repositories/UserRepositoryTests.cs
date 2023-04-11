using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Planner.Data;
using Planner.Models;
using Planner.Repositories;

namespace Planner.UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private PlannerDBContext _context;
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "test@gmail.com";

        [SetUp]
        public void CreateContext()
        {
            _context = CreateDbContext();
        }

        [Test]
        public void Create_ValidUser_ExistInDb()
        {
            var repository = new UserRepository(_context);
            var user = CreateValidUser();

            var userId = repository.Create(user);

            _context.Users.Find(userId).Should().Be(user);
        }

        [TestCase("ro@gmail.com")]
        [TestCase("as@gmail.com")]
        public void CheckIfEmailExist_EmailExist_True(string existEmail)
        {
            MockInMemoryDB();
            var repository = new UserRepository(_context);

            var isExistEmail = repository.CheckIfEmailExist(existEmail);

            isExistEmail.Should().BeTrue();
            CleanInMemoryDB();
        }

        [Test]
        public void CheckIfEmailExist_EmailNotExist_False()
        {
            var repository = new UserRepository(_context);

            var isExistEmail = repository.CheckIfEmailExist(ValidEmail);

            isExistEmail.Should().BeFalse();
        }

        [TestCase("11111111-1111-1111-1111-111111111111")]
        [TestCase("22222222-2222-2222-2222-222222222222")]
        public void GetById_UserExist_NotNullUser(string id)
        {
            MockInMemoryDB();
            var repository = new UserRepository(_context);

            var user = repository.GetById(new Guid(id));

            user.Should().NotBeNull();
            CleanInMemoryDB();
        }

        [Test]
        public void GetById_UserNotExist_Null()
        {
            var repository = new UserRepository(_context);

            var user = repository.GetById(Guid.NewGuid());

            user.Should().BeNull();
        }

        private static User CreateValidUser() 
        => new()
        { 
            Id = Guid.NewGuid(), 
            Name = ValidName, 
            Surname = ValidSurname,
            Email = ValidEmail
        };

        private void MockInMemoryDB()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Rostyk",
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com"
                },
                new User
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Andrew",
                    Surname = "Scherbyna",
                    Email = "as@gmail.com"
                }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        private void CleanInMemoryDB()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }

        private static PlannerDBContext CreateDbContext()
            => new PlannerDBContext(
                new DbContextOptionsBuilder<PlannerDBContext>()
                    .UseInMemoryDatabase(databaseName: "MockPlannerDb")
                    .Options);
    }
}
