using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using Application.Licenses.Queries;
using AutoMapper;
using Moq;

namespace Application.UnitTests.License.Queries
{
    public class GetAllLicensesQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllLicensesQueryHandler _handler;

        public GetAllLicensesQueryTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllLicensesQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnListOfLicenseDto()
        {
            // Arrange
            var query = new GetAllLicensesQuery();
            var licenses = new List<Domain.CVS.Domain.License> { new() { Id = 1 }, new() { Id = 2 } };
            var licenseDtos = new List<LicenseDto> { new() { Id = 1 }, new() { Id = 2 } };

            _unitOfWorkMock.Setup(x => x.LicenseRepository.GetAsync(null, null, "Organization,LicenseHolder", It.IsAny<CancellationToken>()))
                .ReturnsAsync(licenses);
            _mapperMock.Setup(x => x.Map<List<LicenseDto>>(licenses))
                .Returns(licenseDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(licenseDtos.Count, result.Count);
            Assert.Equal(licenseDtos, result);
        }
    }
}
