using Application.Clients.Queries.GetClient;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClient
{
    public class GetClientDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientDtoTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(true);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClient_IdShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(_client.Id);
        }

        [Fact]
        public async Task Handle_GetClient_InitialsShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Initials.Should().Be(_client.Initials);
        }

        [Fact]
        public async Task Handle_GetClient_PrefixLastNameShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.PrefixLastName.Should().Be(_client.PrefixLastName);
        }

        [Fact]
        public async Task Handle_GetClient_LastNameShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.LastName.Should().Be(_client.LastName);
        }

        [Fact]
        public async Task Handle_GetClient_GenderShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            var genderName = Enum.GetName(typeof(Gender), _client.Gender);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Gender.Should().Be(genderName);
        }

        [Fact]
        public async Task Handle_GetClient_StreetNameShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.StreetName.Should().Be(_client.Address.StreetName);
        }

        [Fact]
        public async Task Handle_GetClient_HouseNumberShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.HouseNumber.Should().Be(_client.Address.HouseNumber);
        }

        [Fact]
        public async Task Handle_GetClient_HouseNumberAdditionShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.HouseNumberAddition.Should().Be(_client.Address.HouseNumberAddition);
        }

        [Fact]
        public async Task Handle_GetClient_FirstNameShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.FirstName.Should().Be(_client.FirstName);
        }

        [Fact]
        public async Task Handle_GetClient_PostalCodeShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.PostalCode.Should().Be(_client.Address.PostalCode);
        }

        [Fact]
        public async Task Handle_GetClient_ResidenceShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Residence.Should().Be(_client.Address.Residence);
        }

        [Fact]
        public async Task Handle_GetClient_TelephoneNumberShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.TelephoneNumber.Should().Be(_client.TelephoneNumber);
        }

        [Fact]
        public async Task Handle_GetClient_DateOfBirthShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.DateOfBirth.Should().Be(_client.DateOfBirth);
        }

        [Fact]
        public async Task Handle_GetClient_MaritalStatusShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.MaritalStatus.Should().Be(_client.MaritalStatus.Name);
        }

        [Fact]
        public async Task Handle_GetClient_IsInTargetGroupRegisterShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsInTargetGroupRegister.Should().Be(_client.IsInTargetGroupRegister);
        }

        [Fact]
        public async Task Handle_GetClientOneDriversLicence_DriversLicencesShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "B";
            _client.DriversLicences =
                [
                    new DriversLicence
                    {
                        Category = expectedResult,
                        Description = "Auto rijbewijs"
                    }
                ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.DriversLicences.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClientDriversLicencesIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _client.DriversLicences = [];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.DriversLicences.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Handle_GetClientMultipleDriversLicences_DriversLicencesShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "AM, B";
            _client.DriversLicences =
            [
                new DriversLicence
                {
                    Category = "B",
                    Description = "Auto rijbewijs"
                },
                new DriversLicence
                {
                    Category = "AM",
                    Description = "Brommer rijbewijs"
                }
            ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.DriversLicences.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClientOneDiagnosis_DiagnosesShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "Autisme";
            _client.Diagnoses =
            [
                new Diagnosis
                {
                    Name = expectedResult
                }
            ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Diagnoses.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClientDiagnosesIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _client.Diagnoses = [];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Diagnoses.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Handle_GetClientMultipleDiagnosis_DiagnosesShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "ADHD, Autisme";
            _client.Diagnoses =
            [
                new Diagnosis
                {
                    Name = "Autisme"
                },
                new Diagnosis
                {
                    Name = "ADHD"
                }
            ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Diagnoses.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClientOneBenefitForm_BenefitFormsShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "WIA";
            _client.BenefitForms =
            [
                new BenefitForm
                {
                    Name = expectedResult
                }
            ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.BenefitForm.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClientBenefitFormsIsEmptyList_ShouldReturnEmptyString()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _client.BenefitForms = [];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.BenefitForm.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Handle_GetClientMultipleBenefitForms_BenefitFormShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var expectedResult = "Bijstand, WIA";
            _client.BenefitForms =
            [
                new BenefitForm
                {
                    Name = "WIA"
                },
                new BenefitForm
                {
                    Name = "Bijstand"
                }
            ];

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.BenefitForm.Should().Be(expectedResult);
        }

        [Fact]
        public async Task Handle_GetClient_EmailAddressShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.EmailAddress.Should().Be(_client.EmailAddress);
        }

        [Fact]
        public async Task Handle_GetClient_RemarksShouldBeSet()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Remarks.Should().Be(_client.Remarks);
        }
    }
}
