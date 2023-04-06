using Planner.DTOs;
using Planner.Models;

namespace Planner.Services.Interfaces
{
    public interface IToDoListService
    {
        Guid CreateToDoList(ToDoListDTO toDoListDTO);
        ToDoList GetToDoListById(Guid id);
    }
}
