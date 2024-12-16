using Application.Clients.Queries.GetClient;
using Application.Clients.Queries.GetClientEdit;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Id;

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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
            var client = _testDataGeneratorClient.Create();
            client.Gender = Domain.CVS.Enums.Gender.Woman;
            var expectedResult = (int)Domain.CVS.Enums.Gender.Woman;

            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Gender.Should().Be(expectedResult);
        }

        [Test]
        public async Task TelephoneNumber_IsSet_ShouldReturnTelephoneNumber()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.TelephoneNumber;

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EmailAddress.Should().Be(expectedResult);
        }

        [Test]
        public async Task MaritalStatus_MaritalStatusIsSet_ShouldReturnMaritalStatus()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = new GetClientEditMaritalStatusDto
            {
                Id = client.MaritalStatus.Id,
                Name = client.MaritalStatus.Name
            };


            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.MaritalStatus.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task MaritalStatus_IsNull_ShouldReturnNull()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.MaritalStatus = null;

            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.MaritalStatus.Should().BeNull();
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
        public async Task DriversLicences_IsSet_ShouldReturnDriversLicences()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            var expectedResult = client.DriversLicences.Select(dl => new GetClientEditDriversLicenceDto
            {
                Id = dl.Id,
                Category = dl.Category,
                Description = dl.Description
            });

            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.DriversLicences.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task Diagnoses_IsSet_ShouldReturnDiagnoses()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            var expectedResult = client.Diagnoses.Select(d => new GetClientEditDiagnosisDto
            {
                Id = d.Id,
                Name = d.Name
            });

            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task EmergencyPeople_IsSet_ShouldReturnEmergencyPeople()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            var expectedResult = client.EmergencyPeople.Select(ep => new GetClientEditEmergencyPersonDto
            {
                Name = ep.Name,
                TelephoneNumber = ep.TelephoneNumber
            });
            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EmergencyPeople.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task BenefitForms_IsSet_ShouldReturnBenefitForms()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            var expectedResult = client.BenefitForms.Select(bf => new GetClientEditBenefitFormDto
            {
                Id = bf.Id,
                Name = bf.Name
            });
            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BenefitForms.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task WorkingContracts_IsSet_ShouldReturnWorkingContracts()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            var expectedResult = client.WorkingContracts.Select(o => new GetClientEditWorkingContractDto
            {
                Id = o.Id,
                OrganizationId = o.OrganizationId,
                ContractType = (int)o.ContractType,
                FromDate = o.FromDate,
                ToDate = o.ToDate,
                Function = o.Function
            });
            await AddAsync(client);

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task StreetName_IsSet_ShouldReturnStreetName()
        {
            // Arrange
            var expectedResult = "Dorpstraat";
            var client = _testDataGeneratorClient.Create();
            client.Address = Domain.CVS.ValueObjects.Address.From(expectedResult, 1, string.Empty, "1234 AB", "Arnhem");
            await AddAsync(client);

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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

            var query = new GetClientEditQuery
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
