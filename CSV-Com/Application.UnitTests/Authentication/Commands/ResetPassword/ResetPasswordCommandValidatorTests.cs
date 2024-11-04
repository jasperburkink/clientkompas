using Application.Authentication.Commands.ResetPassword;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Bogus;
using FluentValidation.TestHelper;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidatorTests
    {
        private readonly ResetPasswordCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private static readonly Faker s_faker = FakerConfiguration.Faker;

        public ResetPasswordCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new ResetPasswordCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Handle_SuccessFlow_NoValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Handle_EmailAddressIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = string.Empty,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
        }

        [Fact]
        public async Task Handle_EmailAddressIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = null,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
        }

        [Fact]
        public async Task Handle_EmailAddressIsInvalid_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = "NotAnEmailAddress",
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
        }

        [Fact]
        public async Task Handle_NewPasswordIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = null,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async Task Handle_NewPasswordIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = string.Empty,
                NewPasswordRepeat = password,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async Task Handle_PasswordsAreNotTheSame_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";
            var password2 = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = password2,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public async Task Handle_NewPasswordRepeatIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = null,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPasswordRepeat);
        }

        [Fact]
        public async Task Handle_NewPasswordRepeatIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = string.Empty,
                Token = s_faker.Random.String2(20)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPasswordRepeat);
        }

        [Fact]
        public async Task Handle_TokenIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token);
        }

        [Fact]
        public async Task Handle_TokenIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var password = s_faker.Internet.Password() + "!";

            var command = new ResetPasswordCommand
            {
                EmailAddress = s_faker.Person.Email,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Token);
        }
    }
}
