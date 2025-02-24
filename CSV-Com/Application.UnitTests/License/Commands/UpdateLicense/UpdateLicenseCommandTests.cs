using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Commands.UpdateLicense;
using Application.Licenses.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;
using Moq;

namespace Application.UnitTests.License.Commands.UpdateLicense
{
    public class UpdateLicenseCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UpdateLicenseCommandHandler _handler;

        public UpdateLicenseCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new UpdateLicenseCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnLicenseDto()
        {
            // Arrange
            var command = new UpdateLicenseCommand
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = 1,
                LicenseHolderId = 1,
                Status = LicenseStatus.Active
            };

            var license = new Domain.CVS.Domain.License { Id = command.Id };
            var organization = new Organization { Id = command.OrganizationId };
            var licenseHolder = new User
            {
                Id = command.LicenseHolderId,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "a@b.com",
                TelephoneNumber = "1234567890"
            };
            var licenseDto = new LicenseDto();

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(license);
            _unitOfWorkMock.Setup(x => x.OrganizationRepository.GetByIDAsync(command.OrganizationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(organization);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIDAsync(command.LicenseHolderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(licenseHolder);
            _unitOfWorkMock.Setup(x => x.LicenseRepository.UpdateAsync(It.IsAny<Domain.CVS.Domain.License>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<LicenseDto>(It.IsAny<Domain.CVS.Domain.License>()))
                .Returns(licenseDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(licenseDto, result);
            _unitOfWorkMock.Verify(x => x.LicenseRepository.UpdateAsync(It.IsAny<Domain.CVS.Domain.License>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_LicenseNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateLicenseCommand
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = 1,
                LicenseHolderId = 1,
                Status = LicenseStatus.Active
            };

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.CVS.Domain.License)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_OrganizationNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateLicenseCommand
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = 1,
                LicenseHolderId = 1,
                Status = LicenseStatus.Active
            };

            var license = new Domain.CVS.Domain.License { Id = command.Id };

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(license);
            _unitOfWorkMock.Setup(x => x.OrganizationRepository.GetByIDAsync(command.OrganizationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Organization)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_LicenseHolderNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateLicenseCommand
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = 1,
                LicenseHolderId = 1,
                Status = LicenseStatus.Active
            };

            var license = new Domain.CVS.Domain.License { Id = command.Id };
            var organization = new Organization { Id = command.OrganizationId };

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(license);
            _unitOfWorkMock.Setup(x => x.OrganizationRepository.GetByIDAsync(command.OrganizationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(organization);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIDAsync(command.LicenseHolderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
