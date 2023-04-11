using Planner.Models;

namespace Planner.Repositories.Interfaces
{
    public interface IToDoListRepository
    {
        Guid Create(ToDoList toDoList);
        ToDoList GetById(Guid id);
    }
}
