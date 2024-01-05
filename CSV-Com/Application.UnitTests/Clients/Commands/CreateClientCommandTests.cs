using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;

namespace Application.UnitTests.Clients.Commands
{
    public class CreateClientCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateClientCommandTests()
        {
            _unitOfWorkMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handle_BenifitFormDoesNotExists_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CreateClientCommand();
            var handler = new CreateClientCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
            _unitOfWorkMock.Setup(uw => uw.BenefitFormRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult<BenefitForm>(null));
            _unitOfWorkMock.Setup(uw => uw.MaritalStatusRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult(new MaritalStatus()));

            // Act
            var act = () => handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_MaritalStatusDoesNotExists_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new CreateClientCommand();
            var handler = new CreateClientCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
            _unitOfWorkMock.Setup(uw => uw.BenefitFormRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult(new BenefitForm()));
            _unitOfWorkMock.Setup(uw => uw.MaritalStatusRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult<MaritalStatus>(null));

            // Act
            var act = () => handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }


        [Fact]
        public async Task Handle_SuccessPath_ShouldReturnClientDto()
        {
            // Arrange
            var command = new CreateClientCommand
            {
                FirstName = "FirstName",
                StreetName = "Dorpstraat",
                HouseNumber = 1,
                HouseNumberAddition = "A",
                PostalCode = "1234AB",
                Residence = "Amsterdam"
            };

            var clientDto = new ClientDto
            {
                FirstName = command.FirstName,
                StreetName = command.StreetName,
                HouseNumber = command.HouseNumber,
                HouseNumberAddition = command.HouseNumberAddition,
                PostalCode = command.PostalCode,
                Residence = command.Residence
            };

            var handler = new CreateClientCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
            _unitOfWorkMock.Setup(uw => uw.BenefitFormRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult(new BenefitForm()));
            _unitOfWorkMock.Setup(uw => uw.MaritalStatusRepository.GetByIDAsync(It.IsAny<object>(), default)).Returns(Task.FromResult(new MaritalStatus()));
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.InsertAsync(It.IsAny<Client>(), default));
            _unitOfWorkMock.Setup(uw => uw.SaveAsync(default));
            _mapperMock.Setup(m => m.Map<ClientDto>(It.IsAny<Client>())).Returns(clientDto);

            // Act
            var clientDtoResult = await handler.Handle(command, default);

            // Assert
            clientDto.Should().NotBeNull();
            clientDto.FirstName.Should().Be(command.FirstName);
        }
    }
}
