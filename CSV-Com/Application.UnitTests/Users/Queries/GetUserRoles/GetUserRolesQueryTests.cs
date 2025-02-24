using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Users.Queries.GetUserRoles;
using Moq;

namespace Application.UnitTests.Users.Queries.GetUserRoles
{
    public class GetUserRolesHandlerTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly GetUserRolesHandler _handler;

        public GetUserRolesHandlerTests()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _handler = new GetUserRolesHandler(_identityServiceMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Handle_WhenRolesExist_ShouldReturnMappedRoles()
        {
            // Arrange
            var roles = new List<string> { "Admin", "User", "Manager" };
            _identityServiceMock
                .Setup(service => service.GetAvailableUserRolesAsync())
                .ReturnsAsync(roles);

            var resourceString = "Test";

            _resourceMessageProviderMock
                .Setup(provider => provider.GetMessage<GetUserRolesDto>(It.IsAny<string>()))
                .Returns(resourceString);

            var query = new GetUserRolesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(roles.Count);
            result.Should().BeEquivalentTo(roles.Select(role => new GetUserRolesDto
            {
                Name = resourceString,
                Value = role
            }));
        }

        [Fact]
        public async Task Handle_WhenNoRolesExist_ShouldThrowNotFoundException()
        {
            // Arrange
            _identityServiceMock
                .Setup(service => service.GetAvailableUserRolesAsync())
                .ReturnsAsync([]);

            var query = new GetUserRolesQuery();

            // Act
            var act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("No available userroles found.");
        }
    }
}
