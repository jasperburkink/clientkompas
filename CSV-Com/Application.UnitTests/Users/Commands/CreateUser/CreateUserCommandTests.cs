using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Models;
using Application.Users.Commands.CreateUser;
using AutoMapper;
using Domain.Authentication.Domain;
using Domain.CVS.Domain;
using Moq;

namespace Application.UnitTests.Users.Commands.CreateUser
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly CreateUserCommandHandler _handler;
        private readonly CreateUserCommand _command;
        private readonly User _user;

        public CreateUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _identityServiceMock = new Mock<IIdentityService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _emailServiceMock = new Mock<IEmailService>();

            _user = new User
            {
                Id = 1,
                FirstName = "John",
                PrefixLastName = "van",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "1234567890",
                IsDeactivated = false,
            };

            _command = new CreateUserCommand
            {
                FirstName = "John",
                PrefixLastName = "van",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "1234567890",
                RoleName = "User"
            };

            _mapperMock.Setup(m => m.Map<CreateUserCommandDto>(It.IsAny<User>()))
                .Returns(new CreateUserCommandDto
                {
                    Id = _user.Id,
                    FirstName = _user.FirstName,
                    PrefixLastName = _user.PrefixLastName,
                    LastName = _user.LastName,
                    EmailAddress = _user.EmailAddress,
                    TelephoneNumber = _user.TelephoneNumber
                });

            _identityServiceMock.Setup(s => s.GetCurrentLoggedInUserId())
                .ReturnsAsync(1);

            _unitOfWorkMock.Setup(uw => uw.UserRepository.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(uw => uw.UserRepository.InsertAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uw => uw.SaveAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _passwordServiceMock.Setup(s => s.GeneratePassword(It.IsAny<int>()))
                .Returns("temporaryPassword123");

            _identityServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((Result.Success(), "newUserId"));

            _tokenServiceMock.Setup(s => s.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>()))
                .ReturnsAsync("generatedToken");

            _emailServiceMock.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _handler = new CreateUserCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _passwordServiceMock.Object,
                _emailServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedUserDto()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(_user.Id);
        }

        [Fact]
        public async Task Handle_ShouldCreateCVSUser()
        {
            // Act
            await _handler.Handle(_command, default);

            // Assert
            _unitOfWorkMock.Verify(uw => uw.UserRepository.InsertAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldGeneratePassword()
        {
            // Act
            await _handler.Handle(_command, default);

            // Assert
            _passwordServiceMock.Verify(p => p.GeneratePassword(It.Is<int>(i => i == 8)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallCreateAuthenticationUser()
        {
            // Act
            await _handler.Handle(_command, default);

            // Assert
            _identityServiceMock.Verify(i => i.CreateUserAsync(It.Is<string>(s => s == _command.EmailAddress), It.Is<string>(s => s == "temporaryPassword123"), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldGenerateChangePasswordLink()
        {
            // Act
            await _handler.Handle(_command, default);

            // Assert
            _tokenServiceMock.Verify(t => t.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.Is<string>(s => s == "TemporaryPasswordToken")), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldSendEmailWithPasswordAndLink()
        {
            // Act
            await _handler.Handle(_command, default);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.Is<string>(s => s == _command.EmailAddress), It.Is<string>(s => s == "Gebruikersgegevens"), It.IsAny<string>()), Times.Once);
        }
    }
}
