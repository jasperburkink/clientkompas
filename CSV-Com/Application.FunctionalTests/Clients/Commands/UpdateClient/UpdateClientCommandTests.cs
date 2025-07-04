﻿using Application.Clients.Commands.UpdateClient;
using Application.Clients.Dtos;
using Application.Common.Exceptions;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Domain.Authentication.Constants;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Client;
using TestData.Client.Commands.UpdateClient;
using TestData.Diagnosis;
using TestData.DriversLicence;
using TestData.MaritalStatus;
using TestData.Organization;
using TestData.WorkingContract;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;
        private ITestDataGenerator<MaritalStatus> _testDataGeneratorMartialStatus;
        private ITestDataGenerator<Organization> _testDataGeneratorOrganization;
        private ITestDataGenerator<DriversLicence> _testDataGeneratorDriversLicence;
        private ITestDataGenerator<Diagnosis> _testDataGeneratorDiagnosis;
        private ITestDataGenerator<WorkingContract> _testDataGeneratorWorkingContract;
        private ITestDataGenerator<UpdateClientCommand> _testDataGeneratorUpdateClientCommand;
        private UpdateClientCommand _command;
        private const int NUM_OF_DRIVERSLICENCES = 2, NUM_OF_DIAGNOSES = 2, NUM_OF_WORKINGCONTRACTS = 2;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
            _testDataGeneratorMartialStatus = new MaritalStatusDataGenerator();
            _testDataGeneratorOrganization = new OrganizationDataGenerator();
            _testDataGeneratorDriversLicence = new DriversLicenceDataGenerator();
            _testDataGeneratorDiagnosis = new DiagnosisDataGenerator();
            _testDataGeneratorWorkingContract = new WorkingContractDataGenerator();
            _testDataGeneratorUpdateClientCommand = new UpdateClientCommandDataGenerator();

            _command = _testDataGeneratorUpdateClientCommand.Create();

            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);
            _command.Id = client.Id;

            var maritalStatus = _testDataGeneratorMartialStatus.Create();
            await AddAsync(maritalStatus);
            _command.MaritalStatus!.Id = maritalStatus.Id;

            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var driversLicences = _testDataGeneratorDriversLicence.Create(10);
            foreach (var driversLicence in driversLicences)
            {
                await AddAsync(driversLicence);
            }
            _command.DriversLicences = driversLicences
                .OrderBy(x => Guid.NewGuid())
                .Take(NUM_OF_DRIVERSLICENCES)
                .Select(dl => new DriversLicenceDto { Category = dl.Category, Description = dl.Description, Id = dl.Id }).ToList();

            var diagnoses = _testDataGeneratorDiagnosis.Create(10);
            foreach (var diagnosis in diagnoses)
            {
                await AddAsync(diagnosis);
            }
            _command.Diagnoses = diagnoses
                .OrderBy(x => Guid.NewGuid())
                .Take(NUM_OF_DIAGNOSES)
                .Select(d => new DiagnosisDto { Name = d.Name, Id = d.Id }).ToList();

            var workingContracts = _testDataGeneratorWorkingContract.Create(10);

            _command.WorkingContracts = workingContracts
                .OrderBy(wc => wc.Id)
                .Take(NUM_OF_WORKINGCONTRACTS)
                .Select(wc => new ClientWorkingContractDto
                {
                    Id = wc.Id,
                    ContractType = wc.ContractType,
                    FromDate = wc.FromDate,
                    ToDate = wc.ToDate,
                    Function = wc.Function,
                    OrganizationId = organization.Id
                }).ToList();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldUpdateClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_ClientDoesNotExist_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var command = _command with
            {
                Id = _command.Id + 1
            }; // Client with this id does not exists in the database

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasEmergencyPeople()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.EmergencyPeople)).FirstOrDefault();

            // Assert
            client!.EmergencyPeople.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasDriverLicences()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.DriversLicences)).FirstOrDefault();

            // Assert
            client!.DriversLicences.Should().NotBeNull().And.HaveCount(NUM_OF_DRIVERSLICENCES);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasDiagnoses()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.Diagnoses)).FirstOrDefault();

            // Assert
            client!.Diagnoses.Should().NotBeNull().And.HaveCount(NUM_OF_DIAGNOSES);
        }

        [Test]
        public async Task Handle_CorrectFlow_ClientHasWorkingContracts()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            await SendAsync(_command);
            var client = (await GetAsync<Client>(c => c.WorkingContracts)).FirstOrDefault();

            // Assert
            client!.WorkingContracts.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_MultipleCommands_ShouldUpdateMultipleClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var maritalStatus = _testDataGeneratorMartialStatus.Create();
            await AddAsync(maritalStatus);
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);

            var command2 = _testDataGeneratorUpdateClientCommand.Create();
            command2.Id = client.Id;
            command2.MaritalStatus!.Id = maritalStatus.Id;

            var workingContracts = _testDataGeneratorWorkingContract.Create(10);

            command2.WorkingContracts = workingContracts
               .OrderBy(wc => wc.Id)
               .Take(NUM_OF_WORKINGCONTRACTS)
               .Select(wc => new ClientWorkingContractDto
               {
                   Id = wc.Id,
                   ContractType = wc.ContractType,
                   FromDate = wc.FromDate,
                   ToDate = wc.ToDate,
                   Function = wc.Function,
                   OrganizationId = organization.Id
               }).ToList();

            // Act
            await SendAsync(_command);
            await SendAsync(command2);

            // Assert
            var clients = await GetAsync<Client>();

            clients.Should().NotBeNull().And.HaveCount(2);
        }

        [Test]
        public async Task Handle_FirstNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_FirstNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_FirstNameIsToLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_InitialsIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_InitialsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_InitialsIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PrefixLastNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_LastNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_LastNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_LastNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_GenderIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_StreetNameIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_StreetNameIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_StreetNameIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_HouseNumberIsNegative_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_HouseNumberIsGreaterThanMaxHouseNumber_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_HouseNumberAdditionIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PostalCodeIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PostalCodeContainsSA_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PostalCodeContainsSD_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PostalCodeContainsSS_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_PostalCodeBeginsWithA0_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_ResidenceIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_ResidenceIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_ResidenceIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_TelephoneNumberIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_TelephoneNumberIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_TelephoneNumberIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_DateOfBirthIsInFuture_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_EmailAddressIsInvalid_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_EmailAddressIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_MaritalStatusIsNull_ShouldNotThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_MaritalStatusDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_BenefitFormsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_DriversLicencesIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
            await RunAsAsync(Roles.Administrator);

            var command = _command with
            {
                DriversLicences =
                [
                    new()
                    {
                        Id = 2000,
                        Category = "Foo",
                        Description = "Bar"
                    }
                ]
            };

            // Act
            await SendAsync(command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.DriversLicences.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_DiagnosesIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
            await RunAsAsync(Roles.Administrator);

            var command = _command with
            {
                Diagnoses =
                [
                    new()
                    {
                        Id = 2000,
                        Name = "Foo"
                    }
                ]
            };

            // Act
            await SendAsync(command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.Diagnoses.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_EmergencyPeopleIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_WorkingContractsIsNull_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_RemarksIsTooLong_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_DateOfBirthIsToday_ShouldUpdateClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_DateOfBirthIsInTheFuture_ShouldThrowValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ShouldUpdateClient(string role)
        {
            // Arrange
            await RunAsAsync(role);

            // Act
            await SendAsync(_command);
            var client = (await GetAsync<Client>()).FirstOrDefault();

            // Assert
            client.Should().NotBeNull();
            client!.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ShouldThrowUnauthorizedAccessException()
        {
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => SendAsync(_command));
        }
    }
}
