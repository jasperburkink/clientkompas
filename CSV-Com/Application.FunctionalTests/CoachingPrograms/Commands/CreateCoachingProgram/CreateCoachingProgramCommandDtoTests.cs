using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;
        private ITestDataGenerator<Organization> _testDataGeneratorOrganization;
        private ITestDataGenerator<CreateCoachingProgramCommand> _testDataGeneratorCreateCoachingProgramCommand;
        private CreateCoachingProgramCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
            _testDataGeneratorOrganization = new OrganizationDataGenerator();
            _testDataGeneratorCreateCoachingProgramCommand = new CreateCoachingProgramCommandDataGenerator();

            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);

            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            _command = _testDataGeneratorCreateCoachingProgramCommand.Create();
            _command.ClientId = client.Id;
            _command.OrganizationId = organization.Id;
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldHaveRightClientId()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);
            _command.ClientId = client.Id;

            // Act
            await SendAsync(_command);
            var coachingProgram = (await GetAsync<CoachingProgram>()).FirstOrDefault();

            // Assert
            coachingProgram!.ClientId.Should().Be(client.Id);
        }

        [Test]
        public async Task Handle_Id_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result!.Id.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Handle_ClientFullName_IsSet()
        {
            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);
            _command.ClientId = client.Id;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.ClientFullName.Should().Be(client.FullName);
        }

        [Test]
        public async Task Handle_Title_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Title.Should().Be(_command.Title);
        }

        [Test]
        public async Task Handle_OrderNumber_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.OrderNumber.Should().Be(_command.OrderNumber);
        }

        [Test]
        public async Task Handle_OrganizationName_IsSet()
        {
            // Arrange
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);
            _command.OrganizationId = organization.Id;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.OrganizationName.Should().Be(organization.OrganizationName);
        }

        [Test]
        public async Task Handle_CoachingProgramType_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.CoachingProgramType.Should().Be(_command.CoachingProgramType);
        }

        [Test]
        public async Task Handle_BeginDate_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.BeginDate.Should().Be(_command.BeginDate);
        }

        [Test]
        public async Task Handle_EndDate_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.EndDate.Should().Be(_command.EndDate);
        }

        [Test]
        public async Task Handle_BudgetAmmount_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.BudgetAmmount.Should().Be(_command.BudgetAmmount);
        }

        [Test]
        public async Task Handle_HourlyRate_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.HourlyRate.Should().Be(_command.HourlyRate);
        }
    }
}
