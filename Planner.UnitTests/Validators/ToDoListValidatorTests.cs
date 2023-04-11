using FluentAssertions;
using FluentValidation;
using Planner.DTOs;
using Planner.Validators;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Planner.UnitTests.Validators
{
    [TestFixture]
    public class ToDoListValidatorTests
    {
        private ToDoListValidator _validator;
        private const string ValidName = "test";
        private readonly Guid ValidUserId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _validator = new ToDoListValidator();
        }

        [Test]
        public void ToDoListValidator_ValidData_IsValidShoudBeTrue()
        {
            ToDoListDTO toDoListDTO = new(ValidName, ValidUserId);

            ValidationResult result = _validator.Validate(toDoListDTO);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ToDoListValidatorName_EmptyName_ThrowEmptyNameException()
        {
            ToDoListDTO toDoListDTO = new(string.Empty, ValidUserId);

            Action act = () => _validator.ValidateAndThrow(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name can`t be empty"));
        }

        [TestCase("AnyVeeeeeeeeeeeeryLOOooooooOOOOOoOOoooongNAMEOn50LettersOrLoooooooooonger")]
        public void ToDoListValidatorName_TooManyLettersInName_ThrowMaxLengthNameException(string name)
        {
            ToDoListDTO toDoListDTO = new(name, ValidUserId);

            Action act = () => _validator.ValidateAndThrow(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name must not exceed 50 characters"));
        }

        [Test]
        public void ToDoListValidatorUserId_EmptyUserID_ThrowEmptyUserIdException()
        {
            ToDoListDTO toDoListDTO = new(ValidName, Guid.Empty);

            Action act = () => _validator.ValidateAndThrow(toDoListDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("User id is required"));
        }
    }
}
