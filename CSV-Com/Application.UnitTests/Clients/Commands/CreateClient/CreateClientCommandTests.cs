using System.Linq.Expressions;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using TestData;
using TestData.Client.Commands.CreateClient;

namespace Application.UnitTests.Clients.Commands.CreateClient
{
    public class CreateClientCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly CreateClientCommandHandler _handler;
        private readonly CreateClientCommandValidator _validator;
        private readonly CreateClientCommand _command;

        public CreateClientCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();

            _resourceMessageProviderMock
            .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns("Mock validation message.");

            //_resourceMessageProviderMock
            //.Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
            //.Returns((Type type, string key, object[] args) => $"Mocked message for type: {type}, key: {key}");

            //_resourceMessageProviderMock
            //    .Setup(m => m.GetMessage<int>(It.IsAny<string>(), It.IsAny<object[]>()))
            //    .Returns((string key, object[] args) => $"Mocked message for generic type int, key: {key}");

            ITestDataGenerator<CreateClientCommand> testDataGenerator = new CreateClientCommandDataGenerator();
            _command = testDataGenerator.Create();

            _validator = new CreateClientCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
            _handler = new CreateClientCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.ExistsAsync(
                It.IsAny<Expression<Func<Client, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);
        }

        [Fact]
        public async Task Handle_SuccessPath_ShouldReturnClientDto()
        {
            // Arrange
            var client = new Client()
            {
                FirstName = _command.FirstName,
                Initials = _command.Initials,
                PrefixLastName = _command.PrefixLastName,
                LastName = _command.LastName,
                Gender = _command.Gender.Value,
                Address = Address.From(_command.StreetName, _command.HouseNumber.Value, _command.HouseNumberAddition, _command.PostalCode, _command.Residence),
                TelephoneNumber = _command.TelephoneNumber,
                DateOfBirth = _command.DateOfBirth.Value,
                EmailAddress = _command.EmailAddress,
                BenefitForms = [],
                MaritalStatus = new MaritalStatus
                {
                    Id = _command.MaritalStatus.Id,
                    Name = _command.MaritalStatus.Name
                },
                IsInTargetGroupRegister = false,
                DriversLicences = [],
                Diagnoses = [],
                EmergencyPeople = _command.EmergencyPeople.Select(ep => ep.ToDomainModel(_mapperMock.Object, null)).ToList(),
                WorkingContracts = _command.WorkingContracts.Select(wc => wc.ToDomainModel(_mapperMock.Object, null)).ToList(),
                Remarks = _command.Remarks
            };

            _unitOfWorkMock.Setup(uw => uw.BenefitFormRepository.GetAsync(It.IsAny<Expression<Func<BenefitForm, bool>>>(), It.IsAny<Func<IQueryable<BenefitForm>, IOrderedQueryable<BenefitForm>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client.BenefitForms);
            _unitOfWorkMock.Setup(uw => uw.DiagnosisRepository.GetAsync(It.IsAny<Expression<Func<Diagnosis, bool>>>(), It.IsAny<Func<IQueryable<Diagnosis>, IOrderedQueryable<Diagnosis>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client.Diagnoses);
            _unitOfWorkMock.Setup(uw => uw.DriversLicenceRepository.GetAsync(It.IsAny<Expression<Func<DriversLicence, bool>>>(), It.IsAny<Func<IQueryable<DriversLicence>, IOrderedQueryable<DriversLicence>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client.DriversLicences);

            _unitOfWorkMock.Setup(uw => uw.MaritalStatusRepository.GetAsync(
                It.IsAny<Expression<Func<MaritalStatus, bool>>>(),
                It.IsAny<Func<IQueryable<MaritalStatus>,
                IOrderedQueryable<MaritalStatus>>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync([client.MaritalStatus]);

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.InsertAsync(It.IsAny<Client>(), default));
            _unitOfWorkMock.Setup(uw => uw.SaveAsync(default)).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<ClientDto>(It.IsAny<Client>())).Returns(new ClientDto
            {
                FirstName = client.FirstName,
                StreetName = client.Address.StreetName,
                HouseNumber = client.Address.HouseNumber,
                HouseNumberAddition = client.Address.HouseNumberAddition,
                PostalCode = client.Address.PostalCode,
                Residence = client.Address.Residence
            });

            // Act
            var clientDtoResult = await _handler.Handle(_command, default);

            // Assert
            clientDtoResult.Should().NotBeNull();
            clientDtoResult.FirstName.Should().Be(_command.FirstName);
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
        public async Task Validate_FirstNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { FirstName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FirstName);
        }

        [Fact]
        public async Task Validate_FirstNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { FirstName = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FirstName);
        }

        [Fact]
        public async Task Validate_FirstNameIsWhiteSpace_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { FirstName = "   " };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FirstName);
        }

        [Fact]
        public async Task Validate_FirstNameIsLongerThenAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                FirstName = FakerConfiguration.Faker.Random.String2(ClientConstants.FirstNameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FirstName);
        }

        [Fact]
        public async Task Validate_InitialsIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Initials = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Initials);
        }

        [Fact]
        public async Task Validate_InitialsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Initials = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Initials);
        }

        [Fact]
        public async Task Validate_InitialsIsWhiteSpace_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Initials = "   " };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Initials);
        }

        [Fact]
        public async Task Validate_InitialsIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Initials = FakerConfiguration.Faker.Random.String2(ClientConstants.InitialsMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Initials);
        }

        [Fact]
        public async Task Validate_LastNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { LastName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.LastName);
        }

        [Fact]
        public async Task Validate_LastNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { LastName = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.LastName);
        }

        [Fact]
        public async Task Validate_LastNameIsWhiteSpace_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { LastName = "   " };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.LastName);
        }

        [Fact]
        public async Task Validate_LastNameIsLongerThenAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                LastName = FakerConfiguration.Faker.Random.String2(ClientConstants.LastNameMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.LastName);
        }

        [Fact]
        public async Task Validate_GenderIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { Gender = (Gender)99 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Gender);
        }


        [Fact]
        public async Task Validate_EmailAddressIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { EmailAddress = "invalidemail" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.EmailAddress);
        }

        [Fact]
        public async Task Validate_PostalCodeIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { PostalCode = "1234" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Fact]
        public async Task Validate_PostalCodeContainsSA_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { PostalCode = "1234SA" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Fact]
        public async Task Validate_PostalCodeContainsSD_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { PostalCode = "1234SD" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Fact]
        public async Task Validate_PostalCodeContainsSS_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { PostalCode = "1234SS" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        }

        [Fact]
        public async Task Validate_HouseNumberIsNegative_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { HouseNumber = -1 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.HouseNumber);
        }

        [Fact]
        public async Task Validate_HouseNumberIsGreaterThanMaxHouseNumber_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { HouseNumber = AddressConstants.HouseNumberMaxValue + 1 };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.HouseNumber);
        }

        [Fact]
        public async Task Validate_TelephoneNumberIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { TelephoneNumber = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_TelephoneNumberIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { TelephoneNumber = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_TelephoneNumberIsWhiteSpace_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { TelephoneNumber = "   " };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_TelephoneNumberIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                TelephoneNumber = FakerConfiguration.Faker.Random.String2(ClientConstants.TelephoneNumberMaxLength + 1, "0123456789")
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_DateOfBirthIsDefault_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { DateOfBirth = default };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
        }

        [Fact]
        public async Task Validate_DateOfBirthIsInTheFuture_ShouldHaveValidationError()
        {
            // Arrange
            var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var command = _command with { DateOfBirth = futureDate };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DateOfBirth);
        }


        [Fact]
        public async Task Validate_RemarksIsNull_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = _command with { Remarks = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Remarks);
        }

        [Fact]
        public async Task Validate_RemarksIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Remarks = FakerConfiguration.Faker.Random.String2(ClientConstants.RemarksMaxLength + 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Remarks);
        }

        [Fact]
        public async Task Validate_EmergencyPeopleIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { EmergencyPeople = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.EmergencyPeople);
        }

        [Fact]
        public async Task Validate_EmergencyPeopleIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { EmergencyPeople = [] };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.EmergencyPeople);
        }

        [Fact]
        public async Task Validate_BenefitFormsIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { BenefitForms = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.BenefitForms);
        }

        [Fact]
        public async Task Validate_BenefitFormsIsEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = _command with { BenefitForms = [] };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.BenefitForms);
        }

        [Fact]
        public async Task Validate_DriversLicencesIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { DriversLicences = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DriversLicences);
        }

        [Fact]
        public async Task Validate_DriversLicencesIsEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = _command with { DriversLicences = [] };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.DriversLicences);
        }

        [Fact]
        public async Task Validate_WorkingContractsIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { WorkingContracts = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.WorkingContracts);
        }

        [Fact]
        public async Task Validate_WorkingContractsIsEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var command = _command with { WorkingContracts = [] };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.WorkingContracts);
        }
    }
}
