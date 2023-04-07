using FluentAssertions;
using Moq;
using Planner.DTOs;
using Planner.Models;
using Planner.Repository.Interfaces;
using Planner.Services;
using Task = Planner.Models.Task;

namespace Planner.UnitTests.ServicesTests
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
        private TaskService taskService;

        [SetUp]
        public void Setup()
        {
            taskRepositoryMock = new Mock<ITaskRepository>(MockBehavior.Strict);
            toDoListRepositoryMock = new Mock<IToDoListRepository>(MockBehavior.Strict);
            taskService = new TaskService(taskRepositoryMock.Object, toDoListRepositoryMock.Object);
        }


        [Test]
        public void CreateTask_WithValidData_ReturnNewGuid()
        {
            TaskDTO taskDTO = CreateValidTask();
            SetupRepositoryMockToGetValidToDoListAndCorrectCreateTask();

            var taskId = taskService.CreateTask(taskDTO);

            taskId.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void CreateTask_WithNotExistToDoList_ThrowToDoListNotFoundException()
        {
            TaskDTO taskDTO = CreateValidTask();
            SetupRepositoryMockToTryGetNotExistToDoList();

            Action act = () => taskService.CreateTask(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("ToDoList not found"));
        }

        private void SetupRepositoryMockToGetValidToDoListAndCorrectCreateTask()
        {
            toDoListRepositoryMock.Setup(r => r.GetById(ValidToDoListId)).Returns(ValidToDoList());
            taskRepositoryMock.Setup(r => r.Create(It.IsAny<Task>())).Returns(Guid.NewGuid());
        }

        private void SetupRepositoryMockToTryGetNotExistToDoList()
        {
            toDoListRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<ToDoList>(null);
        }

        private TaskDTO CreateValidTask()
        {
            return new TaskDTO
            (
                ValidName,
                ValidDescription,
                ValidStatus,
                ValidCreated,
                ValidDeadline,
                ValidToDoListId
            );
        }

        private ToDoList ValidToDoList() => new()
        {
            Id = ValidToDoListId,
            Name = ValidName,
            UserId = Guid.NewGuid(),
        };
    }
}
