using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
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

    public class UpdateCoachingProgramCommandDtoTests : BaseTestFixture
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
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldHaveRightId()
        {
            // Act
            await SendAsync(_command);
            var coachingProgram = (await GetAsync<CoachingProgram>()).FirstOrDefault();

            // Assert
            coachingProgram!.Id.Should().Be(_command.Id);
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
            // Arrange
            var title = "Title";
            _command.Title = title;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Title.Should().Be(title);
        }

        [Test]
        public async Task Handle_OrderNumber_IsSet()
        {
            // Arrange
            var orderNumber = "Order1234567";
            _command.OrderNumber = orderNumber;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.OrderNumber.Should().Be(orderNumber);
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
            // Arrange
            var coachingProgramType = CoachingProgramType.ExternJobCoach;
            _command.CoachingProgramType = coachingProgramType;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.CoachingProgramType.Should().Be(coachingProgramType);
        }

        [Test]
        public async Task Handle_BeginDate_IsSet()
        {
            // Arrange
            var beginDate = new DateOnly(1986, 3, 24);
            _command.BeginDate = beginDate;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.BeginDate.Should().Be(beginDate);
        }

        [Test]
        public async Task Handle_EndDate_IsSet()
        {
            // Arrange
            var endDate = new DateOnly(2024, 1, 2);
            _command.EndDate = endDate;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.EndDate.Should().Be(endDate);
        }

        [Test]
        public async Task Handle_BudgetAmmount_IsSet()
        {
            // Arrange
            var budgetAmmount = 100.50m;
            _command.BudgetAmmount = budgetAmmount;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.BudgetAmmount.Should().Be(budgetAmmount);
        }

        [Test]
        public async Task Handle_HourlyRate_IsSet()
        {
            // Arrange
            var hourlyRate = 1000.75m;
            _command.HourlyRate = hourlyRate;

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.HourlyRate.Should().Be(hourlyRate);
        }
    }
}
