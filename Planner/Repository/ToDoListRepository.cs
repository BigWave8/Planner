using Planner.Data;
using Planner.Models;
using Planner.Repository.Interfaces;

namespace Planner.Repository
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly PlannerContext _context;
        public ToDoListRepository(PlannerContext context) 
        { 
            _context = context;
        }
        public Guid Create(ToDoList toDoList)
        {
            _context.Set<ToDoList>().Add(toDoList);
            _context.SaveChanges();
            return toDoList.Id;
        }

        public ToDoList GetById(Guid id)
        {
            return _context.Set<ToDoList>().Find(id);
        }
    }
}
