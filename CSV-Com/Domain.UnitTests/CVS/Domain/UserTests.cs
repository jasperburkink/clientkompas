using Domain.CVS.Domain;

namespace Domain.UnitTests.CVS.Domain
{
    public class UserTests
    {
        private readonly User _user;

        public UserTests()
        {
            _user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "1234567890",
                IsDeactivated = false
            };
        }

        [Fact]
        public void FirstName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var firstName = "Jane";

            // Act
            _user.FirstName = firstName;

            // Assert
            _user.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void PrefixLastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var prefix = "van";

            // Act
            _user.PrefixLastName = prefix;

            // Assert
            _user.PrefixLastName.Should().Be(prefix);
        }

        [Fact]
        public void LastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var lastName = "Smith";

            // Act
            _user.LastName = lastName;

            // Assert
            _user.LastName.Should().Be(lastName);
        }

        [Fact]
        public void EmailAddress_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var email = "jane.smith@example.com";

            // Act
            _user.EmailAddress = email;

            // Assert
            _user.EmailAddress.Should().Be(email);
        }

        [Fact]
        public void TelephoneNumber_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var phone = "0987654321";

            // Act
            _user.TelephoneNumber = phone;

            // Assert
            _user.TelephoneNumber.Should().Be(phone);
        }

        [Fact]
        public void IsDeactivated_SettingProperty_ValueHasBeenSet()
        {
            // Act
            _user.IsDeactivated = true;

            // Assert
            _user.IsDeactivated.Should().BeTrue();
        }

        [Fact]
        public void CreatedByUser_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var createdByUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                EmailAddress = "a@b.com",
                IsDeactivated = false,
                TelephoneNumber = "0134567892"
            };

            // Act
            _user.CreatedByUser = createdByUser;

            // Assert
            _user.CreatedByUser.Should().Be(createdByUser);
        }

        [Fact]
        public void CreatedByUserId_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var createdByUserId = 42;

            // Act
            _user.CreatedByUserId = createdByUserId;

            // Assert
            _user.CreatedByUserId.Should().Be(createdByUserId);
        }
    }
}
