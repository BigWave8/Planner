using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repositories.Interfaces;
using Planner.Services;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private const string ValidName = "test";
        private const string ValidDescription = "test";
        private const Status ValidStatus = Status.ToDo;
        private readonly DateTime ValidCreated = DateTime.Now.AddDays(-1);
        private readonly DateTime ValidDeadline = DateTime.Now.AddDays(1);
        private readonly Guid ValidToDoListId = new("22222222-2222-2222-2222-222222222222");
        private Mock<ITaskRepository> taskRepositoryMock;
        private Mock<IToDoListRepository> toDoListRepositoryMock;
        private Mock<IValidator<TaskDTO>> validatorMock;
        private TaskService taskService;

        [SetUp]
        public void Setup()
        {
            taskRepositoryMock = new Mock<ITaskRepository>(MockBehavior.Strict);
            toDoListRepositoryMock = new Mock<IToDoListRepository>(MockBehavior.Strict);
            validatorMock = new Mock<IValidator<TaskDTO>>(MockBehavior.Strict);
            taskService = new TaskService(taskRepositoryMock.Object, toDoListRepositoryMock.Object, validatorMock.Object);
            SetupValidatorMock();
        }

        [Test]
        public void CreateTask_WithValidData_ReturnNewGuid()
        {
            TaskDTO taskDTO = CreateValidTask(ValidToDoListId);
            SetupTaskRepositoryMock();
            SetupToDoListRepositoryMock(ValidToDoListId, ValidToDoList());

            var taskId = taskService.Create(taskDTO);

            taskId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateTask_WithNotExistToDoList_ThrowToDoListNotFoundException()
        {
            Guid notExistToDoListId = Guid.NewGuid();
            TaskDTO taskDTO = CreateValidTask(notExistToDoListId);
            SetupToDoListRepositoryMock(notExistToDoListId, null);

            Action act = () => taskService.Create(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("ToDoList not found"));
        }

        private void SetupTaskRepositoryMock()
            => taskRepositoryMock.Setup(r => r.Create(It.IsAny<Task>())).Returns(Guid.NewGuid());

        private void SetupToDoListRepositoryMock(Guid id, ToDoList toDoList)
            => toDoListRepositoryMock.Setup(r => r.GetById(id)).Returns(toDoList);

        private void SetupValidatorMock()
            => validatorMock.Setup(v => v.Validate(It.IsAny<TaskDTO>())).Returns(new ValidationResult());

        private TaskDTO CreateValidTask(Guid toDoListId)
        => new
        (
            ValidName,
            ValidDescription,
            ValidStatus,
            ValidCreated,
            ValidDeadline,
            toDoListId
        );
        
        private ToDoList ValidToDoList() 
        => new()
        {
            Id = ValidToDoListId,
            Name = ValidName,
            UserId = Guid.NewGuid(),
        };
    }
}
