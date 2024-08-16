using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandTests : BaseTestFixture
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
        public async Task Handle_CorrectFlow_ShouldCreateCoachingProgram()
        {
            // Act
            await SendAsync(_command);
            var coachingProgram = (await GetAsync<CoachingProgram>()).FirstOrDefault();

            // Assert
            coachingProgram.Should().NotBeNull();
            coachingProgram!.Id.Should().NotBe(0);
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
        public async Task Handle_CorrectFlow_ShouldHaveRightOrganizationId()
        {
            // Arrange
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);
            _command.OrganizationId = organization.Id;

            // Act
            await SendAsync(_command);
            var coachingProgram = (await GetAsync<CoachingProgram>()).FirstOrDefault();

            // Assert
            coachingProgram!.OrganizationId.Should().Be(organization.Id);
        }

        [Test]
        public async Task Handle_MultipleCommands_ShouldCreateMultipleCoachingPrograms()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var command2 = _testDataGeneratorCreateCoachingProgramCommand.Create();

            // Act
            await SendAsync(_command);
            await SendAsync(command2);

            // Assert
            var clients = await GetAsync<Client>();

            clients.Should().NotBeNull().And.HaveCount(2);
        }
    }
}
