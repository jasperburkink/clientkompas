using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Domain.CVS.Domain;
using FluentValidation;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Commands.UpdateCoachingProgram
{
    public class UpdateCoachingProgramCommandValidatorTests : BaseTestFixture
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
        public void Handle_CoachingProgramDoesNotExists_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {

            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
