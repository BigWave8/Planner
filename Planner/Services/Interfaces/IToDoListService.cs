using Planner.DTOs;
using Planner.Models;

namespace Planner.Services.Interfaces
{
    public interface IToDoListService
    {
        Guid Create(ToDoListDTO toDoListDTO);
        ToDoList GetById(Guid id);
    }
}
