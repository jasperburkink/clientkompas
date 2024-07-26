using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using TestData;
using TestData.Client;

namespace Domain.UnitTests.CVS.Domain
{
    public class ClientTests
    {
        private readonly Client _client, _clientDefault;

        public ClientTests()
        {
            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator();
            _client = clientDataGenerator.Create();
            clientDataGenerator.FillOptionalProperties = false;
            _clientDefault = clientDataGenerator.Create();
        }

        [Fact]
        public void FirstName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var firstName = "Jan";

            // Act
            _client.FirstName = firstName;

            // Assert
            _client.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void Initials_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var initials = "J";

            // Act
            _client.Initials = initials;

            // Assert
            _client.Initials.Should().Be(initials);
        }

        [Fact]
        public void PrefixLastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var prefixLastName = "van der";

            // Act
            _client.PrefixLastName = prefixLastName;

            // Assert
            _client.PrefixLastName.Should().Be(prefixLastName);
        }

        [Fact]
        public void LastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var lastName = "Doe";

            // Act
            _client.LastName = lastName;

            // Assert
            _client.LastName.Should().Be(lastName);
        }

        [Fact]
        public void Gender_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var gender = Gender.Woman;

            // Act
            _client.Gender = gender;

            // Assert
            _client.Gender.Should().Be(gender);
        }

        [Fact]
        public void Address_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            string streetName = "Main St", houseNumberAddition = "a", postalCode = "1234 AB", residence = "Arnhem";
            var houseNumber = 4;
            var address = Address.From(streetName, houseNumber, houseNumberAddition, postalCode, residence);

            // Act
            _client.Address = address;

            // Assert
            _client.Address.Should().Be(address);
        }

        [Fact]
        public void TelephoneNumber_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var telephoneNumber = "1234567890";

            // Act
            _client.TelephoneNumber = telephoneNumber;

            // Assert
            _client.TelephoneNumber.Should().Be(telephoneNumber);
        }

        [Fact]
        public void DateOfBirth_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1990, 1, 1);

            // Act
            _client.DateOfBirth = dateOfBirth;

            // Assert
            _client.DateOfBirth.Should().Be(dateOfBirth);
        }

        [Fact]
        public void MaritalStatus_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var maritalStatus = new MaritalStatus
            {
                Name = "Getrouwd"
            };

            // Act
            _client.MaritalStatus = maritalStatus;

            // Assert
            _client.MaritalStatus.Should().Be(maritalStatus);
        }

        [Fact]
        public void EmailAddress_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var emailAddress = "jane.doe@example.com";

            // Act
            _client.EmailAddress = emailAddress;

            // Assert
            _client.EmailAddress.Should().Be(emailAddress);
        }

        [Fact]
        public void DriversLicences_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var driversLicences = new List<DriversLicence>
            {
                new()
                {
                    Category = "B"
                }
            };

            // Act
            _client.DriversLicences = driversLicences;

            // Assert
            _client.DriversLicences.Should().BeSameAs(driversLicences);
        }

        [Fact]
        public void EmergencyPeople_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var emergencyPeople = new List<EmergencyPerson>
            {
                new ()
                {
                    Name = "Jane Doe",
                    TelephoneNumber = "0987654321"
                }
            };

            // Act
            _client.EmergencyPeople = emergencyPeople;

            // Assert
            _client.EmergencyPeople.Should().BeSameAs(emergencyPeople);
        }

        [Fact]
        public void Diagnoses_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var diagnoses = new List<Diagnosis>
            {
                new ()
                {
                    Name = "Autisme"
                }
            };

            // Act
            _client.Diagnoses = diagnoses;

            // Assert
            _client.Diagnoses.Should().BeSameAs(diagnoses);
        }

        [Fact]
        public void BenefitForms_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var benefitForms = new List<BenefitForm>
            {
                new ()
                {
                    Name = "Bijstand"
                }
            };

            // Act
            _client.BenefitForms = benefitForms;

            // Assert
            _client.BenefitForms.Should().BeSameAs(benefitForms);
        }

        [Fact]
        public void WorkingContracts_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var workingContracts = new List<WorkingContract>
            {
                new ()
                {
                    ContractType = ContractType.Temporary,
                    FromDate = new DateOnly(2012, 1, 1),
                    ToDate = new DateOnly(2024, 1, 1),
                    Organization = new Organization(),
                    Function = "Directeur"
                }
            };

            // Act
            _client.WorkingContracts = workingContracts;

            // Assert
            _client.WorkingContracts.Should().BeSameAs(workingContracts);
        }

        [Fact]
        public void Remarks_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var remarks = "Some remarks";

            // Act
            _client.Remarks = remarks;

            // Assert
            _client.Remarks.Should().Be(remarks);
        }

        [Fact]
        public void DeactivationDateTime_Contructor_DefaultValueIsNull()
        {
            // Act
            var deactivationDateTime = _clientDefault.DeactivationDateTime;

            // Assert
            deactivationDateTime.HasValue.Should().Be(false);
            deactivationDateTime.Should().Be(null);
        }


        [Fact]
        public void Deactivate_SuccessState_ReturnsDeactivationDateTime()
        {
            // Arrange
            var now = DateTime.Now;

            // Act
            var result = _client.Deactivate(now);

            // Assert
            result.Should().Be(now);
        }

        [Fact]
        public void Deactivate_SuccessState_DeactivationDateTimeHasBeenSet()
        {
            // Arrange
            var now = DateTime.Now;

            // Act
            _client.Deactivate(now);

            // Assert
            _client.DeactivationDateTime.Should().Be(now);
        }

        [Fact]
        public void FullName_CombinesParts_ReturnNameCombined()
        {
            // Arrange
            _client.FirstName = "Piet";
            _client.PrefixLastName = "van der";
            _client.LastName = "Molen";

            // Act
            var fullName = _client.FullName;

            // Assert
            fullName.Should().Be("Piet van der Molen");
        }

        [Fact]
        public void FullName_WhiteSpacesInNames_ReturnNameWithoutDoubleWhiteSpaces()
        {
            // Arrange
            _client.FirstName = "  Piet  ";
            _client.PrefixLastName = "  van   der  ";
            _client.LastName = "  Molen  ";

            // Act
            var fullName = _client.FullName;

            // Assert
            fullName.Should().Be("Piet van der Molen");
        }

        [Fact]
        public void FullName_PrefixLastNameIsEmpty_ReturnNameWithoutPrefixLastName()
        {
            // Arrange
            _client.FirstName = "Piet";
            _client.PrefixLastName = string.Empty;
            _client.LastName = "Molen";

            // Act
            var fullName = _client.FullName;

            // Assert
            fullName.Should().Be("Piet Molen");
        }

        [Fact]
        public void Deactivate_AlreadyDeactivated_ReturnsInitialDeactivationTime()
        {
            // Arrange
            var initialDeactivationTime = DateTime.Now.AddDays(-1);
            _client.Deactivate(initialDeactivationTime);
            var newDeactivationTime = DateTime.Now;

            // Act
            var result = _client.Deactivate(newDeactivationTime);

            // Assert
            result.Should().Be(initialDeactivationTime);
            _client.DeactivationDateTime.Should().Be(initialDeactivationTime);
        }


    }
}
