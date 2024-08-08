using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;
using TestData.Organization;

namespace Domain.UnitTests.CVS.Domain
{
    public class CoachingProgramTests
    {
        private readonly CoachingProgram _coachingProgram, _coachingProgramDefault;

        public CoachingProgramTests()
        {
            ITestDataGenerator<CoachingProgram> coachingProgramDataGenerator = new CoachingProgramGenerator();
            _coachingProgram = coachingProgramDataGenerator.Create();
            coachingProgramDataGenerator.FillOptionalProperties = false;
            _coachingProgramDefault = coachingProgramDataGenerator.Create();
        }

        [Fact]
        public void Title_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var title = "New Program";

            // Act
            _coachingProgram.Title = title;

            // Assert
            _coachingProgram.Title.Should().Be(title);
        }

        [Fact]
        public void OrderNumber_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var orderNumber = "ORD999";

            // Act
            _coachingProgram.OrderNumber = orderNumber;

            // Assert
            _coachingProgram.OrderNumber.Should().Be(orderNumber);
        }

        [Fact]
        public void CoachingProgramType_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var coachingProgramType = CoachingProgramType.InternJobCoach;

            // Act
            _coachingProgram.CoachingProgramType = coachingProgramType;

            // Assert
            _coachingProgram.CoachingProgramType.Should().Be(coachingProgramType);
        }

        [Fact]
        public void BeginDate_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var beginDate = new DateOnly(2023, 1, 1);

            // Act
            _coachingProgram.BeginDate = beginDate;

            // Assert
            _coachingProgram.BeginDate.Should().Be(beginDate);
        }

        [Fact]
        public void EndDate_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var endDate = new DateOnly(2023, 12, 31);

            // Act
            _coachingProgram.EndDate = endDate;

            // Assert
            _coachingProgram.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void BudgetAmmount_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var budgetAmount = 1000m;

            // Act
            _coachingProgram.BudgetAmmount = budgetAmount;

            // Assert
            _coachingProgram.BudgetAmmount.Should().Be(budgetAmount);
        }

        [Fact]
        public void HourlyRate_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var hourlyRate = 75m;

            // Act
            _coachingProgram.HourlyRate = hourlyRate;

            // Assert
            _coachingProgram.HourlyRate.Should().Be(hourlyRate);
        }

        [Fact]
        public void RemainingHours_CalculatedCorrectly_WhenBudgetAmmountIsSet()
        {
            // Arrange
            var budgetAmount = 1000m;
            var hourlyRate = 50m;

            // Act
            _coachingProgram.BudgetAmmount = budgetAmount;
            _coachingProgram.HourlyRate = hourlyRate;
            var remainingHours = _coachingProgram.RemainingHours;

            // Assert
            remainingHours.Should().Be(budgetAmount / hourlyRate);
        }

        [Fact]
        public void RemainingHours_IsZero_WhenBudgetAmmountIsNull()
        {
            // Arrange
            _coachingProgram.BudgetAmmount = null;
            _coachingProgram.HourlyRate = 50m;

            // Act
            var remainingHours = _coachingProgram.RemainingHours;

            // Assert
            remainingHours.Should().Be(0m);
        }


        [Fact]
        public void Client_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            ITestDataGenerator<Client> testDataGenerator = new ClientDataGenerator();
            var client = testDataGenerator.Create();

            // Act
            _coachingProgram.Client = client;

            // Assert
            _coachingProgram.Client.Should().Be(client);
        }

        [Fact]
        public void ClientId_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var clientId = 1;

            // Act
            _coachingProgram.ClientId = clientId;

            // Assert
            _coachingProgram.ClientId.Should().Be(clientId);
        }

        [Fact]
        public void Organization_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            ITestDataGenerator<Organization> testDataGenerator = new OrganizationDataGenerator();
            var organization = testDataGenerator.Create();

            // Act
            _coachingProgram.Organization = organization;

            // Assert
            _coachingProgram.Organization.Should().Be(organization);
        }

        [Fact]
        public void OrganizationId_SettingProperty_ValueHasBeenSet()
        {
            // Arrange
            var organizationId = 1;

            // Act
            _coachingProgram.OrganizationId = organizationId;

            // Assert
            _coachingProgram.OrganizationId.Should().Be(organizationId);
        }
    }
}
