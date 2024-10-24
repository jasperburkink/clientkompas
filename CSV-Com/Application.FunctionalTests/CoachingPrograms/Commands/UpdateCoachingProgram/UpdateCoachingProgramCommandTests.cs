using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.UpdateCoachingProgram
{
    public class UpdateCoachingProgramCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;
        private ITestDataGenerator<Organization> _testDataGeneratorOrganization;
        private ITestDataGenerator<UpdateCoachingProgramCommand> _testDataGeneratorUpdateCoachingProgramCommand;
        private UpdateCoachingProgramCommand _command;
        private CoachingProgram _coachingProgram;
        private ITestDataGenerator<CoachingProgram> _testDataGeneratorCoachingProgram;

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

            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
            _coachingProgram = _testDataGeneratorCoachingProgram.Create();
            _coachingProgram.ClientId = client.Id;
            _coachingProgram.Client = null;
            _coachingProgram.OrganizationId = organization.Id;
            _coachingProgram.Organization = null;
            await AddAsync(_coachingProgram);

            _command.Id = _coachingProgram.Id;

            await RunAsAsync(Roles.Administrator);
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
        public async Task Handle_MultipleCommands_ShouldUpdateMultipleCoachingPrograms()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            await AddAsync(client);
            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var command2 = _testDataGeneratorUpdateCoachingProgramCommand.Create();
            command2.ClientId = client.Id;
            command2.OrganizationId = organization.Id;

            var coachingProgram = _testDataGeneratorCoachingProgram.Create();
            coachingProgram.ClientId = client.Id;
            coachingProgram.Client = null;
            coachingProgram.OrganizationId = organization.Id;
            coachingProgram.Organization = null;
            await AddAsync(coachingProgram);

            command2.Id = coachingProgram.Id;

            // Act
            await SendAsync(_command);
            await SendAsync(command2);

            // Assert
            var coachingPrograms = await GetAsync<CoachingProgram>();

            coachingPrograms.Should().NotBeNull().And.HaveCount(2);
        }
    }
}
