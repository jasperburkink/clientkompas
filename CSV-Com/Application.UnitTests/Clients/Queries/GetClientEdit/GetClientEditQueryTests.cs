using Application.Clients.Queries.GetClientEdit;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientEditQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientEditQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientEditQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(true);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClientEdit_ShouldReturnClientEditDto()
        {
            // Arrange
            var query = new GetClientEditQuery { ClientId = _client.Id };

            var clientDto = new GetClientEditDto
            {
                FirstName = _client.FirstName,
                LastName = _client.LastName,
                Initials = _client.Initials,
                PrefixLastName = _client.PrefixLastName,
                Id = _client.Id,
                BenefitForms = [.. _client.BenefitForms.Select(bf => new GetClientEditBenefitFormDto { Id = bf.Id, Name = bf.Name })],
                DateOfBirth = _client.DateOfBirth,
                Gender = (int)_client.Gender,
                TelephoneNumber = _client.TelephoneNumber,
                EmailAddress = _client.EmailAddress,
                Diagnoses = [.. _client.Diagnoses.Select(d => new GetClientEditDiagnosisDto { Id = d.Id, Name = d.Name })],
                DriversLicences = [.. _client.DriversLicences.Select(dl => new GetClientEditDriversLicenceDto { Id = dl.Id, Category = dl.Category, Description = dl.Description })],
                EmergencyPeople = [.. _client.EmergencyPeople.Select(ep => new GetClientEditEmergencyPersonDto { Name = ep.Name, TelephoneNumber = ep.TelephoneNumber })],
                HouseNumber = _client.Address.HouseNumber,
                HouseNumberAddition = _client.Address.HouseNumberAddition,
                PostalCode = _client.Address.PostalCode,
                Residence = _client.Address.Residence,
                StreetName = _client.Address.StreetName,
                MaritalStatus = _client.MaritalStatus != null ? new GetClientEditMaritalStatusDto { Id = _client.MaritalStatus.Id, Name = _client.MaritalStatus.Name } : null,
                IsInTargetGroupRegister = false,
                WorkingContracts = [.. _client.WorkingContracts.Select(wc => new GetClientEditWorkingContractDto { Id = wc.Id, OrganizationId = wc.OrganizationId, Function = wc.Function, ContractType = (int)wc.ContractType, FromDate = wc.FromDate, ToDate = wc.ToDate })],
                Remarks = _client.Remarks
            };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clientDto);
        }

        [Fact]
        public async Task Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetClientEditQuery { ClientId = 0 };
            Client client = null;
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(client);

            // Act
            var act = () => _handler.Handle(query, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
