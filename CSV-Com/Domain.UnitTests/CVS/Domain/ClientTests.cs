using Domain.CVS.Domain;

namespace Domain.UnitTests.CVS.Domain
{
    public class ClientTests
    {
        [Fact]
        public void DeactivationDateTime_Contructor_DefaultValueIsNull()
        {
            // Arrange
            var client = new Client();

            // Act
            var deactivationDateTime = client.DeactivationDateTime;

            // Assert
            deactivationDateTime.HasValue.Should().Be(false);
            deactivationDateTime.Should().Be(null);
        }


        [Fact]
        public void Deactivate_SuccessState_ReturnsDeactivationDateTime()
        {
            // Arrange
            var client = new Client();
            var now = DateTime.Now;

            // Act
            var result = client.Deactivate(now);

            // Assert
            result.Should().Be(now);
        }

        [Fact]
        public void Deactivate_SuccessState_DeactivationDateTimeHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var now = DateTime.Now;

            // Act
            client.Deactivate(now);

            // Assert
            client.DeactivationDateTime.Should().Be(now);
        }

        [Fact]
        public void FullName_CombinesParts_ReturnNameCombined()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "Piet",
                PrefixLastName = "van der",
                LastName = "Molen"
            };

            // Act
            var fullName = client.FullName;

            // Assert
            fullName.Should().Be("Piet van der Molen");
        }

        [Fact]
        public void FullName_WhiteSpacesInNames_ReturnNameWithoutDoubleWhiteSpaces()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "  Piet  ",
                PrefixLastName = "  van   der  ",
                LastName = "  Molen  "
            };

            // Act
            var fullName = client.FullName;

            // Assert
            fullName.Should().Be("Piet van der Molen");
        }

        [Fact]
        public void FullName_PrefixLastNameIsEmpty_ReturnNameWithoutPrefixLastName()
        {
            // Arrange
            var client = new Client
            {
                FirstName = "Piet",
                PrefixLastName = string.Empty,
                LastName = "Molen"
            };

            // Act
            var fullName = client.FullName;

            // Assert
            fullName.Should().Be("Piet Molen");
        }
    }
}
