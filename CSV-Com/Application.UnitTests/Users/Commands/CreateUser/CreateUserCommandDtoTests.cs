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
    public class CreateUserCommandDtoTests
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

        public CreateUserCommandDtoTests()
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

            _unitOfWorkMock.Setup(uw => uw.UserRepository.AnyAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(uw => uw.UserRepository.InsertAsync(It.IsAny<User>(), default))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uw => uw.SaveAsync(default))
                .Returns(Task.CompletedTask);

            _passwordServiceMock.Setup(s => s.GeneratePassword(It.IsAny<int>()))
                .Returns("temporaryPassword123");

            _identityServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((Result.Success(), "newUserId"));

            _identityServiceMock.Setup(s => s.AddUserToRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Success());

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
        public async Task Handle_ShouldMapUserToDto()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            var userDto = result.Value;
            userDto.Id.Should().Be(_user.Id);
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
        public async Task Handle_ShouldCallMapperWithUser()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            _mapperMock.Verify(m => m.Map<CreateUserCommandDto>(It.Is<User>(u => u.EmailAddress == _user.EmailAddress)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldGeneratePassword()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            _passwordServiceMock.Verify(p => p.GeneratePassword(It.Is<int>(i => i == 8)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallSendEmail()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            _emailServiceMock.Verify(e => e.SendEmailAsync(It.Is<string>(s => s == _command.EmailAddress), It.Is<string>(s => s == "Gebruikersgegevens"), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateAuthenticationUser()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            _identityServiceMock.Verify(i => i.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldGenerateChangePasswordLink()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            _tokenServiceMock.Verify(t => t.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.Is<string>(s => s == "TemporaryPasswordToken")), Times.Once);
        }
    }
}
