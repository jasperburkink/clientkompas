using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Commands.BlockLicense;
using Domain.CVS.Enums;
using MediatR;
using Moq;

namespace Application.UnitTests.License.Commands.BlockLicense
{
    public class BlockLicenseCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BlockLicenseCommandHandler _handler;

        public BlockLicenseCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new BlockLicenseCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_LicenseExists_BlocksLicense()
        {
            // Arrange
            var licenseId = 1;
            var license = new Domain.CVS.Domain.License { Id = licenseId, Status = LicenseStatus.Active };
            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(licenseId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(license);

            var command = new BlockLicenseCommand(licenseId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(LicenseStatus.Blocked, license.Status);
            _unitOfWorkMock.Verify(x => x.LicenseRepository.UpdateAsync(license, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_LicenseNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var licenseId = 1;
            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(licenseId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.CVS.Domain.License)null);

            var command = new BlockLicenseCommand(licenseId);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
