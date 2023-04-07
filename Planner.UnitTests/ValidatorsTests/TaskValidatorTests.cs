using FluentAssertions;
using FluentValidation;
using Planner.DTOs;
using Planner.Models;
using Planner.Validators;

namespace Planner.UnitTests.ValidatorsTests
{
    [TestFixture]
    public class TaskValidatorTests
    {
        private TaskValidator validator;
        private const string ValidName = "test";
        private const string ValidDescription = "test";
        private const Status ValidStatus = Status.ToDo; //В мене може бути кілька валідних статусів, чи окей юзати лиш один?
        private readonly DateTime ValidCreated = DateTime.Now.AddDays(-1);
        private readonly DateTime ValidDeadline = DateTime.Now.AddDays(1);
        private readonly Guid ValidToDoListId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            validator = new TaskValidator();
        }


        [Test]
        public void TaskValidator_ValidData_IsValidShoudBeTrue()
        {
            TaskDTO taskDTO = CreateTaskDTOWith();

            var result = validator.Validate(taskDTO);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void TaskValidatorName_EmptyName_ThrowEmptyNameException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(name: string.Empty);

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name can`t be empty"));
        }

        [Test]
        public void TaskValidatorName_TooManyLettersInName_ThrowMaxLengthNameException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(name: new string('a', 51));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name must not exceed 50 characters"));
        }

        [Test]
        public void TaskValidatorDescription_TooManyLettersInDescription_ThrowMaxLengthDescriptionException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(description: new string('a', 1001));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Description must not exceed 1000 characters"));
        }

        [TestCase(Status.Completed)]
        [TestCase(Status.Skipped)]
        [TestCase(Status.Archived)]
        public void TaskValidatorStatus_NotValidStatusForCreateTask_ThrowStatusValidationException(Status status)
        {
            TaskDTO taskDTO = CreateTaskDTOWith(status: status);

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("On create status must be ToDo or InProgress or ForLater"));
        }

        [Test]
        public void TaskValidatorCreated_EmptyCreatedTime_ThrowEmptyCreatedException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(created: DateTime.MinValue);

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Created time must be specified"));
        }

        [Test]
        public void TaskValidatorCreated_TooEarlyCreatedTime_ThrowTooEarlyCreatedException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(created: new DateTime(DateTime.Now.Year - 2, 12, 31));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Task must be created no earlier than last year"));
        }

        [Test]
        public void TaskValidatorCreated_CreatedInFuture_ThrowTooLateCreatedException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(created: DateTime.Now.AddMinutes(1));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Created time can`t be in the future"));
        }

        [Test]
        public void TaskValidatorDeadline_EarlierThanCreated_ThrowDeadlineException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(deadline: ValidCreated.AddSeconds(-1));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Deadline must be later than created time"));
        }

        [Test]
        public void TaskValidatorDeadline_TooGreaterThanCreated_ThrowDeadlineException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(deadline: ValidCreated.AddYears(1).AddDays(1));

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Deadline should not exceed one year from created time"));
        }

        [Test]
        public void TaskValidatorToDoListId_EmptyToDoListId_ThrowEmptyToDoListIdException()
        {
            TaskDTO taskDTO = CreateTaskDTOWith(toDoListId: Guid.Empty);

            Action act = () => validator.ValidateAndThrow(taskDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("ToDoList id is required"));
        }

        private TaskDTO CreateTaskDTOWith(
            string name = ValidName,
            string description = ValidDescription, 
            Status status = ValidStatus, 
            DateTime? created = null, 
            DateTime? deadline = null, 
            Guid? toDoListId = null)
        {
            if (created == null) created = ValidCreated;
            if (deadline == null) deadline = ValidDeadline;
            if (toDoListId == null) toDoListId = ValidToDoListId;

            return new(name, description, status, (DateTime)created, deadline, (Guid)toDoListId);
        }
    }
}
