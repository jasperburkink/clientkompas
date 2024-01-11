using Domain.CVS.Exceptions;
using Domain.CVS.ValueObjects;

namespace Domain.UnitTests.CVS.ValueObjects
{
    public class AddressTests
    {
        private const string StreetNameCorrect = "Dorpstraat";
        private const int HouseNumberCorrect = 1;
        private const string HouseNumerAdditionCorrect = "a";
        private const string PostalCodeCorrect = "1234AB";
        private const string ResidenceCorrect = "Amsterdam";

        [Fact]
        public void From_AllDataIsCorrect_ShouldReturnAddressWithValues()
        {
            // Arrange
            Address address;

            // Act
            address = Address.From(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Assert
            address.Should().NotBeNull();
            address.StreetName.Should().Be(StreetNameCorrect);
            address.HouseNumber.Should().Be(HouseNumberCorrect);
            address.HouseNumberAddition.Should().Be(HouseNumerAdditionCorrect);
            address.PostalCode.Should().Be(PostalCodeCorrect);
            address.Residence.Should().Be(ResidenceCorrect);
        }

        [Fact]
        public void From_DataIsInCorrect_ShouldThrowInvalidAddressException()
        {
            // Arrange
            Address address;

            // Act
            Action act = () => Address.From("", 0, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Assert
            act.Should().Throw<InvalidAddressException>();
        }

        [Fact]
        public void IsValid_AllDataIsCorrect_ShouldReturnTrue()
        {
            // Arrange
            var address = new Address(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_StreetNameIsEmpty_ShouldReturnFalse()
        {
            // Arrange
            var streetname = string.Empty;

            var address = new Address(streetname, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_StreetNameIsNull_ShouldReturnFalse()
        {
            // Arrange
            string streetname = null;

            var address = new Address(streetname, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_HouseNumberIs0_ShouldReturnFalse()
        {
            // Arrange
            var houseNumber = 0;

            var address = new Address(StreetNameCorrect, houseNumber, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_HouseNumberAdditionIsEmpty_ShouldReturnTrue()
        {
            // Arrange
            var houseNumerAddition = string.Empty;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, houseNumerAddition, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_HouseNumberAdditionIsNull_ShouldReturnTrue()
        {
            // Arrange
            string houseNumerAddition = null;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, houseNumerAddition, PostalCodeCorrect, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_PostalCodeIsEmpty_ShouldReturnFalse()
        {
            // Arrange
            var postalCode = string.Empty;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, postalCode, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_PostalCodeIsNull_ShouldReturnFalse()
        {
            // Arrange
            string postalCode = null;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, postalCode, ResidenceCorrect);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ResidenceIsEmpty_ShouldReturnFalse()
        {
            // Arrange
            var residence = string.Empty;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, residence);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_ResidenceIsNull_ShouldReturnFalse()
        {
            // Arrange
            string residence = null;

            var address = new Address(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, residence);

            // Act
            var isValid = address.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void GetFormattedAddress_WithValidAddress_ShouldReturnFormattedString()
        {
            // Arrange
            var address = Address.From(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect); ;
            var formattedAddressStringCorrect = $"{StreetNameCorrect} {HouseNumberCorrect} {HouseNumerAdditionCorrect}, {PostalCodeCorrect} {ResidenceCorrect}";

            // Act
            var formattedAddressString = address.GetFormattedAddress();

            // Assert
            formattedAddressString.Should().Be(formattedAddressStringCorrect);
        }

        [Fact]
        public void ToString_WithValidAddress_ShouldReturnFormattedString()
        {
            // Arrange
            var address = Address.From(StreetNameCorrect, HouseNumberCorrect, HouseNumerAdditionCorrect, PostalCodeCorrect, ResidenceCorrect); ;
            var formattedAddressStringCorrect = $"{StreetNameCorrect} {HouseNumberCorrect} {HouseNumerAdditionCorrect}, {PostalCodeCorrect} {ResidenceCorrect}";

            // Act
            var formattedAddressString = address.ToString();

            // Assert
            formattedAddressString.Should().Be(formattedAddressStringCorrect);
        }
    }
}
