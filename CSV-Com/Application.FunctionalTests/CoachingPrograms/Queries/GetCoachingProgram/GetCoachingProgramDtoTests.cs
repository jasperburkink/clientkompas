using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Domain.CVS.Domain;
using TestData;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgram
{
    public class GetCoachingProgramDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<CoachingProgram> _testDataGeneratorCoachingProgram;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
        }


        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Id;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task ClientFullName_IsSet_ShouldReturnClientFullName()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Client.FullName;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.ClientFullName.Should().Be(expectedResult);
        }

        [Test]
        public async Task Title_IsSet_ShouldReturnTitle()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Title;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Title.Should().Be(expectedResult);
        }

        [Test]
        public async Task OrderNumber_IsSet_ShouldReturnOrderNumber()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.OrderNumber;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.OrderNumber.Should().Be(expectedResult);
        }

        [Test]
        public async Task OrganizationName_IsSet_ShouldReturnOrganizationName()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Organization!.OrganizationName;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.OrganizationName.Should().Be(expectedResult);
        }

        [Test]
        public async Task CoachingProgramType_IsSet_ShouldReturnCoachingProgramType()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();
            var coachingProgramType = "Prive jobcoach traject";
            coachingProgram.CoachingProgramType = Domain.CVS.Enums.CoachingProgramType.PrivateCoachingProgram;

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.CoachingProgramType.Should().Be(coachingProgramType);
        }

        [Test]
        public async Task BeginDate_IsSet_ShouldReturnBeginDate()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.BeginDate;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BeginDate.Should().Be(expectedResult);
        }

        [Test]
        public async Task EndDate_IsSet_ShouldReturnEndDate()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.EndDate;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EndDate.Should().Be(expectedResult);
        }

        [Test]
        public async Task BudgetAmmount_IsSet_ShouldReturnBudgetAmmount()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.BudgetAmmount;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.BudgetAmmount.Should().Be(expectedResult);
        }

        [Test]
        public async Task HourlyRate_IsSet_ShouldReturnHourlyRate()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.HourlyRate;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.HourlyRate.Should().Be(expectedResult);
        }

        [Test]
        public async Task RemainingHours_IsSet_ShouldReturnRemainingHours()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.RemainingHours;

            var query = new GetCoachingProgramQuery
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.RemainingHours.Should().Be(expectedResult);
        }
    }
}
