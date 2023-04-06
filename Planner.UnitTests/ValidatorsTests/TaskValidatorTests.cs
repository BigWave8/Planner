using FluentAssertions;
using FluentValidation;
using Planner.DTOs;
using Planner.Models;
using Planner.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            TaskDTO taskDTO = new(ValidName, ValidDescription, ValidStatus, ValidCreated, ValidDeadline, ValidToDoListId);

            var result = validator.Validate(taskDTO);

            result.IsValid.Should().BeTrue();
        }
    }
}
