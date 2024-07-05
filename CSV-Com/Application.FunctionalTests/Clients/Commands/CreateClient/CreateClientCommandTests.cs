using Application.Clients.Commands.CreateClient;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentValidation;
using TestData;
using TestData.Client.Commands.CreateClient;
using TestData.Diagnosis;
using TestData.DriversLicence;
using TestData.MaritalStatus;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Commands.CreateClient
{
    public class CreateClientCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<MaritalStatus> _testDataGeneratorMartialStatus;
        private ITestDataGenerator<Organization> _testDataGeneratorOrganization;
        private ITestDataGenerator<DriversLicence> _testDataGeneratorDriversLicence;
        private ITestDataGenerator<Diagnosis> _testDataGeneratorDiagnosis;
        private ITestDataGenerator<CreateClientCommand> _testDataGeneratorCreateClientCommand;
        private CreateClientCommand _command;
        private const int NumOfDriversLicences = 2, NumOfDiagnoses = 2;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorMartialStatus = new MaritalStatusDataGenerator();
            _testDataGeneratorOrganization = new OrganizationDataGenerator();
            _testDataGeneratorDriversLicence = new DriversLicenceDataGenerator();
            _testDataGeneratorDiagnosis = new DiagnosisDataGenerator();
            _testDataGeneratorCreateClientCommand = new CreateClientCommandDataGenerator();

            var maritalStatus = _testDataGeneratorMartialStatus.Create();
            await AddAsync(maritalStatus);

            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var driversLicences = _testDataGeneratorDriversLicence.Create(10);
            foreach (var driversLicence in driversLicences)
            {
                await AddAsync(driversLicence);
            }

            var diagnoses = _testDataGeneratorDiagnosis.Create(10);
            foreach (var diagnosis in diagnoses)
            {
                await AddAsync(diagnosis);
            }

            _command = _testDataGeneratorCreateClientCommand.Create();
            _command.MaritalStatus!.Id = maritalStatus.Id;
            foreach (var workingContract in _command.WorkingContracts)
            {
                workingContract.OrganizationId = organization.Id;
            }

            _command.DriversLicences = driversLicences.OrderBy(x => Guid.NewGuid()).Take(NumOfDriversLicences).Select(dl => new DriversLicenceDto { Category = dl.Category, Description = dl.Description, Id = dl.Id }).ToList();
            _command.Diagnoses = diagnoses.OrderBy(x => Guid.NewGuid()).Take(NumOfDiagnoses).Select(d => new DiagnosisDto { Name = d.Name, Id = d.Id }).ToList();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldCreateClient()
        {
            // Arrange
            var firstName = _command.FirstName;
            var lastName = _command.LastName;

            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.FirstName.Should().Be(firstName);
            client.LastName.Should().Be(lastName);
            client.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasEmergencyPeople()
        {
            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.EmergencyPeople)).FirstOrDefault();

            // Assert
            client!.EmergencyPeople.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasDriverLicences()
        {
            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.DriversLicences)).FirstOrDefault();

            // Assert
            client!.DriversLicences.Should().NotBeNull().And.HaveCount(NumOfDriversLicences);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasDiagnoses()
        {
            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.Diagnoses)).FirstOrDefault();

            // Assert
            client!.Diagnoses.Should().NotBeNull().And.HaveCount(NumOfDiagnoses);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasWorkingContracts()
        {
            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.WorkingContracts)).FirstOrDefault();

            // Assert
            client!.WorkingContracts.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_MultipleCommands_ShouldCreateMultipleClients()
        {
            // Arrange
            var maritalStatus = _testDataGeneratorMartialStatus.Create();
            await AddAsync(maritalStatus);
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var command2 = _testDataGeneratorCreateClientCommand.Create();
            command2.MaritalStatus!.Id = maritalStatus.Id;
            foreach (var workingContract in command2.WorkingContracts)
            {
                workingContract.OrganizationId = organization.Id;
            }

            // Act
            await SendAsync(_command);
            await SendAsync(command2);

            // Assert
            var clients = await GetAsync<Client>();

            clients.Should().NotBeNull().And.HaveCount(2);
        }

        [Test]
        public void Handle_FirstNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                FirstName = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_FirstNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                FirstName = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_FirstNameIsToLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                FirstName = FakerConfiguration.Faker.Random.String2(ClientConstants.FirstNameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_InitialsIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Initials = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_InitialsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Initials = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_InitialsIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Initials = FakerConfiguration.Faker.Random.String2(ClientConstants.InitialsMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PrefixLastNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PrefixLastName = FakerConfiguration.Faker.Random.String2(ClientConstants.PrefixLastNameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_LastNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                LastName = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_LastNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                LastName = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_LastNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                LastName = FakerConfiguration.Faker.Random.String2(ClientConstants.LastNameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_GenderIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Gender = (Gender)99
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_StreetNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                StreetName = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_StreetNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                StreetName = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_StreetNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                StreetName = FakerConfiguration.Faker.Random.String2(AddressConstants.StreetnameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_HouseNumberIsNegative_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HouseNumber = -1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_HouseNumberIsGreaterThanMaxHouseNumber_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HouseNumber = AddressConstants.HouseNumberMaxValue + 1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_HouseNumberAdditionIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HouseNumberAddition = FakerConfiguration.Faker.Random.String2(AddressConstants.HouseNumberAdditionMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PostalCodeIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PostalCode = "1234"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PostalCodeContainsSA_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PostalCode = "1234SA"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PostalCodeContainsSD_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PostalCode = "1234SD"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PostalCodeContainsSS_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PostalCode = "1234SS"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PostalCodeBeginsWithA0_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                PostalCode = "0234SA"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_ResidenceIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Residence = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_ResidenceIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Residence = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_ResidenceIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Residence = FakerConfiguration.Faker.Random.String2(AddressConstants.ResidenceMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_TelephoneNumberIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                TelephoneNumber = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_TelephoneNumberIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                TelephoneNumber = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_TelephoneNumberIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                TelephoneNumber = FakerConfiguration.Faker.Random.String2(ClientConstants.TelephoneNumberMaxLength + 1, "0123456789")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_DateOfBirthIsInFuture_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_EmailAddressIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = "invalid.email"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_EmailAddressIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_MaritalStatusIsNull_ShouldNotThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                MaritalStatus = null
            };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_MaritalStatusDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = _command with
            {
                MaritalStatus = new MaritalStatusDto
                {
                    Id = 5,
                    Name = "Foo"
                }
            };

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_BenefitFormsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                BenefitForms = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_DriversLicencesIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                DriversLicences = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Handle_DriversLicenceDoesNotExist_DriversLicenceIsNotAddedToClient()
        {
            // Arrange
            var command = _command with
            {
                DriversLicences = new List<DriversLicenceDto>()
                {
                    new()
                    {
                        Id = 2000,
                        Category = "Foo",
                        Description = "Bar"
                    }
                }
            };

            // Act
            await SendAsync(command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.DriversLicences.Should().BeEmpty();
        }

        [Test]
        public void Handle_DiagnosesIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Diagnoses = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Handle_DiagnosisDoesNotExist_DiagnosisIsNotAddedToClient()
        {
            // Arrange
            var command = _command with
            {
                Diagnoses = new List<DiagnosisDto>()
                {
                    new()
                    {
                        Id = 2000,
                        Name = "Foo"
                    }
                }
            };

            // Act
            await SendAsync(command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.Diagnoses.Should().BeEmpty();
        }

        [Test]
        public void Handle_EmergencyPeopleIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmergencyPeople = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_WorkingContractsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                WorkingContracts = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_RemarksIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Remarks = FakerConfiguration.Faker.Random.String2(ClientConstants.RemarksMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_DateOfBirthIsToday_ShouldCreateClient()
        {
            // Arrange
            var command = _command with
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_DateOfBirthIsInTheFuture_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
