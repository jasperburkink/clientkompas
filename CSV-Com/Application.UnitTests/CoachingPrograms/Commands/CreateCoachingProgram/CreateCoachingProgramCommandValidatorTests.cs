using System.Linq.Expressions;
using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentValidation.TestHelper;
using Moq;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;

namespace Application.UnitTests.CoachingPrograms.Commands.CreateCoachingProgram
{
    public class CreateCoachingProgramCommandValidatorTests
    {
        private readonly CreateCoachingProgramCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly CreateCoachingProgramCommand _command;

        public CreateCoachingProgramCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.ExistsAsync(
                It.IsAny<Expression<Func<Client, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);
            _unitOfWorkMock.Setup(uw => uw.OrganizationRepository.ExistsAsync(
               It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<CancellationToken>()
               )).ReturnsAsync(true);

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? new object[0])}");

            _validator = new CreateCoachingProgramCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator();
            var client = clientDataGenerator.Create();

            ITestDataGenerator<Organization> organizationDataGenerator = new OrganizationDataGenerator();
            var organization = organizationDataGenerator.Create();

            ITestDataGenerator<CreateCoachingProgramCommand> testDataGenerator = new CreateCoachingProgramCommandDataGenerator();
            _command = testDataGenerator.Create();
            _command.ClientId = client.Id;
            _command.OrganizationId = organization.Id;
        }

        [Fact]
        public async Task Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_ClientDoesNotExists_ShouldHaveValidationError()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.ExistsAsync(
               It.IsAny<Expression<Func<Client, bool>>>(),
                   It.IsAny<CancellationToken>()
               )).ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.ClientId);
        }

        [Fact]
        public async Task Validate_ClientIdIs0_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { ClientId = 0 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.ClientId);
        }

        [Fact]
        public async Task Validate_TitleIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Title = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Title);
        }

        [Fact]
        public async Task Validate_TitleIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Title = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Title);
        }

        [Fact]
        public async Task Validate_TitleIsWhiteSpace_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Title = "   " };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Title);
        }

        [Fact]
        public async Task Validate_TitleIsLongerThenAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Title = FakerConfiguration.Faker.Random.String2(CoachingProgramConstants.TITLE_MAXLENGTH + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Title);
        }

        [Fact]
        public async Task Validate_OrderNumberIsLongerThenAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                OrderNumber = FakerConfiguration.Faker.Random.String2(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.OrderNumber);
        }

        [Fact]
        public async Task Validate_OrganizationDoesNotExists_ShouldHaveValidationError()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.OrganizationRepository.ExistsAsync(
               It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<CancellationToken>()
               )).ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.OrganizationId);
        }

        [Fact]
        public async Task Validate_CoachingProgramTypeIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { CoachingProgramType = (CoachingProgramType)99 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.CoachingProgramType);
        }

        [Fact]
        public async Task Validate_BeginDateInFuture_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { BeginDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.BeginDate);
        }

        [Fact]
        public async Task Validate_EndDateInFuture_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.EndDate);
        }


        [Fact]
        public async Task Validate_BeginDateAfterEndDate_ShouldHaveValidationError()
        {
            // Arrange
            var endDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(2)));
            var beginDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)));
            var command = _command with { EndDate = endDate, BeginDate = beginDate };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.BeginDate);
        }

        [Fact]
        public async Task Validate_EndDateBeforeBeginDate_ShouldHaveValidationError()
        {
            // Arrange
            var beginDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(1)));
            var endDate = DateOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromDays(2)));
            var command = _command with { EndDate = endDate, BeginDate = beginDate };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.EndDate);
        }

        [Fact]
        public async Task Validate_BudgetAmmountHasNegativeValue_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { BudgetAmmount = -1 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.BudgetAmmount);
        }

        [Fact]
        public async Task Validate_HourlyRateIs0_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { HourlyRate = 0 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.HourlyRate);
        }

        [Fact]
        public async Task Validate_HourlyRateHasNegativeValue_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { HourlyRate = -1 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.HourlyRate);
        }
    }
}
