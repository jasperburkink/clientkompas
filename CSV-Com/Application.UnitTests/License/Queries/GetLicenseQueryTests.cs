using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using Application.Licenses.Queries;
using AutoMapper;
using Moq;

namespace Application.UnitTests.License.Queries
{
    public class GetLicenseQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetLicenseQueryHandler _handler;

        public GetLicenseQueryTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetLicenseQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnLicenseDto()
        {
            // Arrange
            var query = new GetLicenseQuery(1);
            var license = new Domain.CVS.Domain.License { Id = query.Id };
            var licenseDto = new LicenseDto();

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(query.Id, "Organization,LicenseHolder", It.IsAny<CancellationToken>()))
                .ReturnsAsync(license);
            _mapperMock.Setup(x => x.Map<LicenseDto>(license))
                .Returns(licenseDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(licenseDto, result);
        }

        [Fact]
        public async Task Handle_LicenseNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetLicenseQuery(1);

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetByIDAsync(query.Id, "Organization,LicenseHolder", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.CVS.Domain.License)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
