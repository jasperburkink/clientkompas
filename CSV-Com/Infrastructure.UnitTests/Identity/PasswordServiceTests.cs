using FluentAssertions;
using Infrastructure.Identity;

namespace Infrastructure.UnitTests.Identity
{
    public class PasswordServiceTests
    {
        [Fact]
        public void GeneratePassword_CorrectFlow_PasswordIsNotNullOrEmpty()
        {
            // Arrange
            var passwordService = new PasswordService();
            var length = 10;

            // Act
            var password = passwordService.GeneratePassword(length);

            // Assert
            password.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GeneratePassword_CorrectFlow_PasswordIsValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var length = 10;

            // Act
            var password = passwordService.GeneratePassword(length);
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void GeneratePassword_ValidLength4_PasswordLengthIs4()
        {
            // Arrange
            var passwordService = new PasswordService();
            var length = 10;

            // Act
            var password = passwordService.GeneratePassword(length);

            // Assert
            password.Length.Should().Be(length);
        }

        [Fact]
        public void GeneratePassword_LengthIs2_ThrowsArgumentException()
        {
            // Arrange
            var passwordService = new PasswordService();
            var length = 2;

            // Act
            var act = () => passwordService.GeneratePassword(length);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsValid_CorrectFlow_IsValidIsTrue()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "Test!1234";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_PasswordDoesNotContainNumber_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "Test!erwrwr";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_PasswordDoesNotContainUppercaseLetter_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "test!1234";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }


        [Fact]
        public void IsValid_PasswordDoesNotContainLowercaseLetter_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "TEST!1234";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_PasswordDoesNotContainSpecialCharacter_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "Test1234";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_PasswordIsToLong_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = """
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234Test1234
                            """;

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_PasswordIsToShort_PasswordIsInValid()
        {
            // Arrange
            var passwordService = new PasswordService();
            var password = "Test";

            // Act
            var isValid = passwordService.IsValidPassword(password);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
