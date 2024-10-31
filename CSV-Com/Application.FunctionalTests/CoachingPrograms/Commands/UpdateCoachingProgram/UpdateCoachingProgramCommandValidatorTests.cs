using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
using Application.Common.Exceptions;
using Domain.Authentication.Constants;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.UpdateCoachingProgram
{
    public class UpdateCoachingProgramCommandValidatorTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;
        private ITestDataGenerator<Organization> _testDataGeneratorOrganization;
        private ITestDataGenerator<UpdateCoachingProgramCommand> _testDataGeneratorUpdateCoachingProgramCommand;
        private UpdateCoachingProgramCommand _command;
        private CoachingProgram _coachingProgram;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
            _testDataGeneratorOrganization = new OrganizationDataGenerator();
            _testDataGeneratorUpdateCoachingProgramCommand = new UpdateCoachingProgramCommandDataGenerator();

            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);

            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            _command = _testDataGeneratorUpdateCoachingProgramCommand.Create();
            _command.ClientId = client.Id;
            _command.OrganizationId = organization.Id;

            ITestDataGenerator<CoachingProgram> testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
            _coachingProgram = testDataGeneratorCoachingProgram.Create();
            _coachingProgram.ClientId = client.Id;
            _coachingProgram.Client = null;
            _coachingProgram.OrganizationId = organization.Id;
            _coachingProgram.Organization = null;
            await AddAsync(_coachingProgram);

            _command.Id = _coachingProgram.Id;

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public void Handle_CorrectFlow_NoValidationExcceptions()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(() => SendAsync(_command));
        }

        [Test]
        public void Handle_CoachingProgramDoesNotExists_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Id = _command.Id + 1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_ClientIs0_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                ClientId = 0
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_ClientDoesNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                ClientId = _command.ClientId + 1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_TitleEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Title = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_TitleTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Title = FakerConfiguration.Faker.Random.String2(CoachingProgramConstants.TITLE_MAXLENGTH + 1)
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_OrderNumberTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                OrderNumber = FakerConfiguration.Faker.Random.String2(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH + 1)
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_OrganizationDoesNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                ClientId = _command.ClientId + 1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_OrganizationIsNull_NoValidationExcceptions()
        {
            // Arrange
            _command.OrganizationId = null;

            // Act & Assert
            Assert.DoesNotThrowAsync(() => SendAsync(_command));
        }

        [Test]
        public void Handle_CoachingProgramTypeInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                CoachingProgramType = (CoachingProgramType)99
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_BeginDateAfterEndDate_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                BeginDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2))
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_BeginDateIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                BeginDate = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_EndDateBeforeBeginDate_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                BeginDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_EndDateIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EndDate = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_BudgetAmmountNegative_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                BudgetAmmount = -1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_HourlyRateIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HourlyRate = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_HourlyRateIs0_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HourlyRate = 0
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_HourlyRateNegative_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                HourlyRate = -1
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
