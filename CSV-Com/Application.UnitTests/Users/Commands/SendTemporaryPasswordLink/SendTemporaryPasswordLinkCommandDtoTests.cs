using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Users.Commands.SendTemporaryPasswordLink;
using AutoMapper;
using Domain.CVS.Domain;
using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.UnitTests.Users.Commands.SendTemporaryPasswordLink
{
    public class SendTemporaryPasswordLinkCommandDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SendTemporaryPasswordLinkCommandHandler _handler;
        private readonly AuthenticationUser _authUser;
        private readonly User _cvsUser;

        public SendTemporaryPasswordLinkCommandDtoTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _identityServiceMock = new Mock<IIdentityService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _emailServiceMock = new Mock<IEmailService>();
            _configurationMock = new Mock<IConfiguration>();

            _authUser = new AuthenticationUser
            {
                Id = "authUserId",
                CVSUserId = 1,
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com",
                HasTemporaryPassword = true,
                TemporaryPasswordTokenCount = 0
            };

            _cvsUser = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "0123456789",
                CreatedByUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    EmailAddress = "admin@example.com",
                    TelephoneNumber = "0123456789",
                }
            };

            _identityServiceMock.Setup(s => s.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(_authUser);

            _tokenServiceMock.Setup(t => t.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync([new TemporaryPasswordToken { CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(7), IsRevoked = false, IsUsed = false }]);

            _unitOfWorkMock.Setup(uw => uw.UserRepository.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([_cvsUser]);

            var sectionMock = new Mock<IConfigurationSection>();
            sectionMock.Setup(x => x.Value).Returns("https://testurl.com/ChangePassword/");
            _configurationMock.Setup(x => x.GetSection("Urls:ChangePassword")).Returns(sectionMock.Object);

            _handler = new SendTemporaryPasswordLinkCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _passwordServiceMock.Object,
                _emailServiceMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidUser_ShouldReturnMappedDto()
        {
            // Arrange
            var command = new SendTemporaryPasswordLinkCommand { UserId = _authUser.Id };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Value.UserId.Should().Be(_cvsUser.Id);
        }

        [Fact]
        public async Task Handle_NullUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            var command = new SendTemporaryPasswordLinkCommand { UserId = null };

            // Act
            var act = () => _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldReturnFailure()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.UserRepository.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            var command = new SendTemporaryPasswordLinkCommand { UserId = _authUser.Id };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("User cannot be null.");
        }
    }
}
