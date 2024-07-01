using Application.Clients.Commands.DeactivateClient;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandTests
    {

        [Fact]
        public async Task Handle_CorrectFlow_ReturnsClientWithDeactivationDateTime()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var clientRepositoryMock = new Mock<IRepository<Client>>();
            var client = new Client();

            unitOfWorkMock.Setup(uow => uow.ClientRepository).Returns(clientRepositoryMock.Object);
            clientRepositoryMock.Setup(cr => cr.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(client);

            var handler = new DeactivateClientCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new DeactivateClientCommand { Id = 1 };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            client.DeactivationDateTime.Should().NotBeNull();
            client.DeactivationDateTime!.Value.Date.Should().Be(DateTime.Now.Date);
            clientRepositoryMock.Verify(cr => cr.UpdateAsync(It.IsAny<Client>(), CancellationToken.None), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var clientRepositoryMock = new Mock<IRepository<Client>>();
            Client client = null;

            unitOfWorkMock.Setup(uow => uow.ClientRepository).Returns(clientRepositoryMock.Object);
            clientRepositoryMock.Setup(cr => cr.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(client);

            var handler = new DeactivateClientCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new DeactivateClientCommand { Id = 1 };

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ClientIsAlreadyDeactivated_ThrowsInvalidOperationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var clientRepositoryMock = new Mock<IRepository<Client>>();
            var client = new Client();
            client.SetPrivate(c => c.DeactivationDateTime, new DateTime(2020, 04, 01));

            unitOfWorkMock.Setup(uow => uow.ClientRepository).Returns(clientRepositoryMock.Object);
            clientRepositoryMock.Setup(cr => cr.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(client);

            var handler = new DeactivateClientCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new DeactivateClientCommand { Id = 1 };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
            clientRepositoryMock.Verify(cr => cr.GetByIDAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_SaveChangesFailure_ThrowsDbUpdateException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var clientRepositoryMock = new Mock<IRepository<Client>>();
            var client = new Client();

            unitOfWorkMock.Setup(uow => uow.ClientRepository).Returns(clientRepositoryMock.Object);
            clientRepositoryMock.Setup(cr => cr.GetByIDAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(client);
            unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateException("Failed to save changes"));

            var handler = new DeactivateClientCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new DeactivateClientCommand { Id = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
