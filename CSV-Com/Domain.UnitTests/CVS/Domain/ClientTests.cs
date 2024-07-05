using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;

namespace Domain.UnitTests.CVS.Domain
{
    public class ClientTests
    {

        [Fact]
        public void FirstName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var firstName = "Jan";

            // Act
            client.FirstName = firstName;

            // Assert
            client.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void Initials_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var initials = "J";

            // Act
            client.Initials = initials;

            // Assert
            client.Initials.Should().Be(initials);
        }

        [Fact]
        public void PrefixLastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var prefixLastName = "van der";

            // Act
            client.PrefixLastName = prefixLastName;

            // Assert
            client.PrefixLastName.Should().Be(prefixLastName);
        }

        [Fact]
        public void LastName_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var lastName = "Doe";

            // Act
            client.LastName = lastName;

            // Assert
            client.LastName.Should().Be(lastName);
        }

        [Fact]
        public void Gender_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var gender = Gender.Woman;

            // Act
            client.Gender = gender;

            // Assert
            client.Gender.Should().Be(gender);
        }

        [Fact]
        public void Address_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            string streetName = "Main St", houseNumberAddition = "a", postalCode = "1234 AB", residence = "Arnhem";
            var houseNumber = 4;
            var address = Address.From(streetName, houseNumber, houseNumberAddition, postalCode, residence);

            // Act
            client.Address = address;

            // Assert
            client.Address.Should().Be(address);
        }

        [Fact]
        public void TelephoneNumber_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var telephoneNumber = "1234567890";

            // Act
            client.TelephoneNumber = telephoneNumber;

            // Assert
            client.TelephoneNumber.Should().Be(telephoneNumber);
        }

        [Fact]
        public void DateOfBirth_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var dateOfBirth = new DateOnly(1990, 1, 1);

            // Act
            client.DateOfBirth = dateOfBirth;

            // Assert
            client.DateOfBirth.Should().Be(dateOfBirth);
        }

        [Fact]
        public void MaritalStatus_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var maritalStatus = new MaritalStatus
            {
                Name = "Getrouwd"
            };

            // Act
            client.MaritalStatus = maritalStatus;

            // Assert
            client.MaritalStatus.Should().Be(maritalStatus);
        }

        [Fact]
        public void EmailAddress_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var emailAddress = "jane.doe@example.com";

            // Act
            client.EmailAddress = emailAddress;

            // Assert
            client.EmailAddress.Should().Be(emailAddress);
        }

        [Fact]
        public void DriversLicences_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var driversLicences = new List<DriversLicence>
            {
                new()
                {
                    Category = "B"
                }
            };

            // Act
            client.DriversLicences = driversLicences;

            // Assert
            client.DriversLicences.Should().BeSameAs(driversLicences);
        }

        [Fact]
        public void EmergencyPeople_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var emergencyPeople = new List<EmergencyPerson>
            {
                new ()
                {
                    Name = "Jane Doe",
                    TelephoneNumber = "0987654321"
                }
            };

            // Act
            client.EmergencyPeople = emergencyPeople;

            // Assert
            client.EmergencyPeople.Should().BeSameAs(emergencyPeople);
        }

        [Fact]
        public void Diagnoses_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var diagnoses = new List<Diagnosis>
            {
                new ()
                {
                    Name = "Autisme"
                }
            };

            // Act
            client.Diagnoses = diagnoses;

            // Assert
            client.Diagnoses.Should().BeSameAs(diagnoses);
        }

        [Fact]
        public void BenefitForms_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var benefitForms = new List<BenefitForm>
            {
                new ()
                {
                    Name = "Bijstand"
                }
            };

            // Act
            client.BenefitForms = benefitForms;

            // Assert
            client.BenefitForms.Should().BeSameAs(benefitForms);
        }

        [Fact]
        public void WorkingContracts_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
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
            client.WorkingContracts = workingContracts;

            // Assert
            client.WorkingContracts.Should().BeSameAs(workingContracts);
        }

        [Fact]
        public void Remarks_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var client = new Client();
            var remarks = "Some remarks";

            // Act
            client.Remarks = remarks;

            // Assert
            client.Remarks.Should().Be(remarks);
        }


        [Fact]
        public void DriversLicences_DefaultValue_IsEmptyList()
        {
            // Arrange
            var client = new Client();

            // Act
            var driversLicences = client.DriversLicences;

            // Assert
            driversLicences.Should().BeEmpty();
        }

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

        [Fact]
        public void Deactivate_AlreadyDeactivated_ReturnsInitialDeactivationTime()
        {
            // Arrange
            var client = new Client();
            var initialDeactivationTime = DateTime.Now.AddDays(-1);
            client.Deactivate(initialDeactivationTime);
            var newDeactivationTime = DateTime.Now;

            // Act
            var result = client.Deactivate(newDeactivationTime);

            // Assert
            result.Should().Be(initialDeactivationTime);
            client.DeactivationDateTime.Should().Be(initialDeactivationTime);
        }


    }
}
