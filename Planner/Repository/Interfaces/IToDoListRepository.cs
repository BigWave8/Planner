using Planner.Models;

namespace Planner.Repository.Interfaces
{
    public interface IToDoListRepository
    {
        Guid Create(ToDoList toDoList);
        ToDoList GetById(Guid id);
    }
}
