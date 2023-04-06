using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Models;
using Planner.Repository;

namespace Planner.UnitTests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        public PlannerContext _context;
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "test@gmail.com";
        private const string ExistEmail = "ro@gmail.com";

        [SetUp]
        public void CreateContext()
        {
            var builder = new DbContextOptionsBuilder<PlannerContext>();
            builder.UseInMemoryDatabase(databaseName: "MockPlannerDb");

            _context = new PlannerContext(builder.Options);

            var users = new List<User>
            {
                new User 
                { 
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Name = "Rostyk",
                    Surname = "Oryshchak",
                    Email = "ro@gmail.com",
                    ToDoList = new List<ToDoList>()
                },
                new User 
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    Name = "Andrew",
                    Surname = "Scherbyna",
                    Email = "as@gmail.com",
                    ToDoList = new List<ToDoList>()
                }
            };

            _context.User.AddRange(users);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.SaveChanges();
        }


        [Test]
        public void Create_ValidUser_ExistInDb()
        {
            var repository = new UserRepository(_context);
            var user = CreateValidUser();

            var userId = repository.Create(user);

            _context.Set<User>().Find(userId).Should().Be(user);
        }

        [Test]
        public void CheckIfEmailExist_EmailExist_True()
        {
            var repository = new UserRepository(_context);

            var isExistEmail = repository.CheckIfEmailExist(ExistEmail);

            isExistEmail.Should().BeTrue();
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
            var repository = new UserRepository(_context);

            var user = repository.GetById(new Guid(id));

            user.Should().NotBeNull();
        }

        [Test]
        public void GetById_UserNotExist_Null()
        {
            var repository = new UserRepository(_context);

            var user = repository.GetById(Guid.NewGuid());

            user.Should().BeNull();
        }

        private static User CreateValidUser() => new()
        { 
            Id = Guid.NewGuid(), 
            Name = ValidName, 
            Surname = ValidSurname,
            Email = ValidEmail, 
            ToDoList = new List<ToDoList>() 
        };
    }
}
