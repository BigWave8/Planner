using Planner.Data;
using Planner.Models;
using Planner.Repositories.Interfaces;

namespace Planner.Repositories
{
    internal class ToDoListRepository : IToDoListRepository
    {
        private readonly PlannerDBContext _context;

        public ToDoListRepository(PlannerDBContext context) 
        { 
            _context = context;
        }

        public Guid Create(ToDoList toDoList)
        {
            _context.ToDoLists.Add(toDoList);
            _context.SaveChanges();
            return toDoList.Id;
        }

        public ToDoList GetById(Guid id)
            => _context.ToDoLists.Find(id);
    }
}
