using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandTests
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

            var organization = _testDataGeneratorOrganization.Create();
            await AddAsync(organization);


            _command = _testDataGeneratorCreateCoachingProgramCommand.Create();
        }


        [Test]
        public async Task Handle_CorrectFlow_ShouldCreateCoachingProgram()
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
    }
}
