using Application.Clients.Queries.GetClient;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClient
{
    public class GetClientDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
        }

        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Id;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task FirstName_IsSet_ShouldReturnFirstName()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.FirstName;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.FirstName.Should().Be(expectedResult);
        }

        [Test]
        public async Task Initials_IsSet_ShouldReturnInitials()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Initials;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Initials.Should().Be(expectedResult);
        }

        [Test]
        public async Task PrefixLastName_IsSet_ShouldReturnPrefixLastName()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.PrefixLastName;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.PrefixLastName.Should().Be(expectedResult);
        }

        [Test]
        public async Task LastName_IsSet_ShouldReturnLastName()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.LastName;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.LastName.Should().Be(expectedResult);
        }

        [Test]
        public async Task Gender_GenderIsSet_ShouldReturnName()
        {
            // Arrange
            var expectedResult = "Woman";
            var client = _testDataGeneratorClient.Create();
            client.Gender = Domain.CVS.Enums.Gender.Woman;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Gender.Should().NotBeNull().And.Be(expectedResult);
        }

        [Test]
        public async Task TelephoneNumber_IsSet_ShouldReturnTelephoneNumber()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.TelephoneNumber;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.TelephoneNumber.Should().Be(expectedResult);
        }

        [Test]
        public async Task DateOfBirth_IsSet_ShouldReturnDateOfBirth()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.DateOfBirth;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DateOfBirth.Should().Be(expectedResult);
        }

        [Test]
        public async Task EmailAddress_IsSet_ShouldReturnEmailAddress()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.EmailAddress;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EmailAddress.Should().Be(expectedResult);
        }

        [Test]
        public async Task MaritalStatus_MaritalStatusIsSet_ShouldReturnName()
        {
            // Arrange
            var expectedResult = "Ongehuwd";
            var client = _testDataGeneratorClient.Create();
            client.MaritalStatus = new MaritalStatus
            {
                Name = expectedResult
            };

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.MaritalStatus.Should().NotBeNull().And.Be(expectedResult);
        }

        [Test]
        public async Task MaritalStatus_IsNull_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.MaritalStatus = null;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.MaritalStatus.Should().Be(string.Empty);
        }

        [Test]
        public async Task IsInTargetGroupRegister_IsTrue_ShouldReturnIsInTargetGroupRegister()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();
            var expectedResult = true;
            client.IsInTargetGroupRegister = expectedResult;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.IsInTargetGroupRegister.Should().Be(expectedResult);
        }

        [Test]
        public async Task IsInTargetGroupRegister_IsFalse_ShouldReturnIsInTargetGroupRegister()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();
            var expectedResult = false;
            client.IsInTargetGroupRegister = expectedResult;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.IsInTargetGroupRegister.Should().Be(expectedResult);
        }

        [Test]
        public async Task DriversLicences_OneDriversLicence_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "B";
            var client = _testDataGeneratorClient.Create();
            client.DriversLicences =
            [
                new DriversLicence
                {
                    Category = expectedResult,
                    Description = "Auto rijbewijs"
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DriversLicences.Should().Be(expectedResult);
        }

        [Test]
        public async Task DriversLicences_DriversLicencesIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.DriversLicences = [];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DriversLicences.Should().Be(string.Empty);
        }

        [Test]
        public async Task DriversLicences_DriversLicencesIsNull_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.DriversLicences = null;
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DriversLicences.Should().Be(string.Empty);
        }

        [Test]
        public async Task DriversLicences_MultipleDriversLicences_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "AM, B";
            var client = _testDataGeneratorClient.Create();
            client.DriversLicences =
            [
                new DriversLicence
                {
                    Category = "B",
                    Description = "Auto rijbewijs"
                },
                new DriversLicence
                {
                    Category = "AM",
                    Description = "Brommer rijbewijs"
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DriversLicences.Should().Be(expectedResult);
        }

        [Test]
        public async Task Diagnoses_OneDiagnosis_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "Autisme";
            var client = _testDataGeneratorClient.Create();
            client.Diagnoses =
            [
                new Diagnosis
                {
                    Name = expectedResult
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.Should().Be(expectedResult);
        }

        [Test]
        public async Task Diagnoses_DiagnosesIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.Diagnoses = [];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.Should().Be(string.Empty);
        }

        [Test]
        public async Task Diagnoses_DiagnosesIsNull_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.Diagnoses = null;
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.Should().Be(string.Empty);
        }

        [Test]
        public async Task Diagnoses_MultipleDiagnosis_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "ADHD, Autisme";
            var client = _testDataGeneratorClient.Create();
            client.Diagnoses =
            [
                new Diagnosis
                {
                    Name = "Autisme"
                },
                new Diagnosis
                {
                    Name = "ADHD"
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.Should().Be(expectedResult);
        }



        [Test]
        public async Task BenefitForms_OneBenefitForm_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "WIA";
            var client = _testDataGeneratorClient.Create();
            client.BenefitForms =
            [
                new BenefitForm
                {
                    Name = expectedResult
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BenefitForm.Should().Be(expectedResult);
        }

        [Test]
        public async Task BenefitForms_BenefitFormsIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.BenefitForms = [];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BenefitForm.Should().Be(string.Empty);
        }

        [Test]
        public async Task BenefitForms_BenefitFormsIsNull_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.BenefitForms = null;
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BenefitForm.Should().Be(string.Empty);
        }

        [Test]
        public async Task BenefitForms_MultipleBenefitForms_ShouldReturnFormattedString()
        {
            // Arrange
            var expectedResult = "Bijstand, WIA";
            var client = _testDataGeneratorClient.Create();
            client.BenefitForms =
            [
                new BenefitForm
                {
                    Name = "WIA"
                },
                new BenefitForm
                {
                    Name = "Bijstand"
                }
            ];
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BenefitForm.Should().Be(expectedResult);
        }

        [Test]
        public async Task StreetName_IsSet_ShouldReturnStreetName()
        {
            // Arrange
            var expectedResult = "Dorpstraat";
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From(expectedResult, 1, string.Empty, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.StreetName.Should().Be(expectedResult);
        }

        [Test]
        public async Task HouseNumber_IsSet_ShouldReturnHouseNumber()
        {
            // Arrange
            var expectedResult = 1;
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", expectedResult, string.Empty, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.HouseNumber.Should().Be(expectedResult);
        }

        [Test]
        public async Task HouseNumberAddition_IsSet_ShouldReturnHouseNumberAddition()
        {
            // Arrange
            var expectedResult = "a";
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", 1, expectedResult, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.HouseNumberAddition.Should().Be(expectedResult);
        }

        [Test]
        public async Task HouseNumberAddition_IsNull_ShouldReturnEmptyString()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", 1, null, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.HouseNumberAddition.Should().Be(string.Empty);
        }

        [Test]
        public async Task HouseNumberAddition_IsEmptyString_ShouldReturnEmptyString()
        {
            // Arrange
            var expectedResult = string.Empty;
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", 1, expectedResult, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.HouseNumberAddition.Should().Be(expectedResult);
        }

        [Test]
        public async Task PostalCode_IsSet_ShouldReturnPostalCode()
        {
            // Arrange
            var expectedResult = "1234 AB";
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", 1, string.Empty, expectedResult, "Arnhem");
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.PostalCode.Should().Be(expectedResult);
        }

        [Test]
        public async Task Residence_IsSet_ShouldReturnResidence()
        {
            // Arrange
            var expectedResult = "Arnhem";
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From("Dorpstraat", 1, string.Empty, "1234 AB", expectedResult);
            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Residence.Should().Be(expectedResult);
        }

        [Test]
        public async Task Remarks_IsSet_ShouldReturnRemarks()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Remarks;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Remarks.Should().Be(expectedResult);
        }
    }
}
