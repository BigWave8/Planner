using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Planner.DTOs;
using Planner.Validators;

namespace Planner.UnitTests.Validators
{
    [TestFixture]
    public class UserValidatorTests
    {
        private UserValidator _validator;
        private const string ValidName = "test";
        private const string ValidSurname = "test";
        private const string ValidEmail = "test@gmail.com";

        [SetUp]
        public void SetUp()
        {
            _validator = new UserValidator();
        }


        [Test]
        public void UserValidator_ValidData_IsValidShoudBeTrue()
        {
            UserDTO userDTO = new(ValidName, ValidSurname, ValidEmail);

            ValidationResult result = _validator.Validate(userDTO);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void UserValidatorName_EmptyName_ThrowEmptyNameException()
        {
            UserDTO userDTO = new(string.Empty, ValidSurname, ValidEmail);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name can`t be empty"));
        }

        [TestCase("T")]
        [TestCase("Tttttttttttttttttttttttttttttttttttttttttttt")]
        public void UserValidatorName_IncorrectNameLength_ThrowLengthNameException(string name)
        {
            UserDTO userDTO = new(name, ValidSurname, ValidEmail);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Name must be between 2 and 30 characters"));
        }

        [Test]
        public void UserValidatorSurname_EmptySurname_ThrowEmptySurnameException()
        {
            UserDTO userDTO = new(ValidName, string.Empty, ValidEmail);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Surname can`t be empty"));
        }

        [TestCase("O")]
        [TestCase("ooooooooooooooooooooooooooooooooooooooooo")]
        public void UserValidatorSurname_IncorrectSurnameLength_ThrowLengthSurnameException(string surname)
        {
            UserDTO userDTO = new(ValidName, surname, ValidEmail);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Surname must be between 2 and 30 characters"));
        }

        [TestCase("Test1488")]
        [TestCase("007")]
        [TestCase("?")]
        [TestCase("1")]
        [TestCase("   ")]
        [TestCase("321go!")]
        [TestCase("Rost_O")]
        [TestCase("Big~Wave")]
        [TestCase(" WithSpaces ")]
        [TestCase("With Space")]
        public void UserValidatorSurname_SurnameContainsNotOnlyLetters_ThrowSurnameMatchesException(string surname)
        {
            UserDTO userDTO = new(ValidName, surname, ValidEmail);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Surname must contain only letters"));
        }

        [Test]
        public void UserValidatorEmail_EmptyEmail_ThrowEmptyEmailException()
        {
            UserDTO userDTO = new(ValidName, ValidSurname, string.Empty);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("Email can`t be empty"));
        }

        [TestCase("Rostyk")]
        [TestCase("@")]
        [TestCase("rostyk.com")]
        [TestCase("ro_gmail.com")]
        [TestCase("   ")]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("RO0gmail.com")]
        [TestCase("@gmail.com")]
        /* [TestCase(" @gmail.com")] */
        public void UserValidatorEmail_NotEmail_ThrowNotEmailException(string email)
        {
            UserDTO userDTO = new(ValidName, ValidSurname, email);

            Action act = () => _validator.ValidateAndThrow(userDTO);

            act.Should().Throw<Exception>().Where(e => e.Message.Contains("It's not like email"));
        }
    }
}
