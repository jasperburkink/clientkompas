using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Infrastructure.Identity;
using Moq;
using TestData;
using TestData.User;

namespace Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserQueryDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly ITestDataGenerator<User> _userTestDataGenerator;
        private readonly IMapper _mapper;

        public GetUserQueryDtoTests()
        {
            _userTestDataGenerator = new UserDataGenerator();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _identityServiceMock = new Mock<IIdentityService>();

            var configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task Handle_CorrectFlow_ShouldReturnDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task Handle_UserDoesNotExists_DtoShouldBeNull()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default));

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task Handle_DtoFirstName_FirstNameIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.FirstName.Should().Be(user.FirstName);
        }

        [Fact]
        public async Task Handle_DtoPrefixLastName_PrefixLastNameIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();
            user.PrefixLastName = "van der";

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.PrefixLastName.Should().Be(user.PrefixLastName);
        }

        [Fact]
        public async Task Handle_DtoLastName_LastNameIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.LastName.Should().Be(user.LastName);
        }

        [Fact]
        public async Task Handle_DtoTelephoneNumber_TelephoneNumberIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.TelephoneNumber.Should().Be(user.TelephoneNumber);
        }

        [Fact]
        public async Task Handle_DtoFullName_FullNameIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();
            user.PrefixLastName = "van der";

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.FullName.Should().Be(user.FullName);
        }

        [Fact]
        public async Task Handle_DtoEmailAddress_EmailAddressIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.EmailAddress.Should().Be(user.EmailAddress);
        }

        [Fact]
        public async Task Handle_DtoDeactivationDateTime_DeactivationDateTimeIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();
            user.Deactivate(DateTime.UtcNow);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.DeactivationDateTime.Should().Be(user.DeactivationDateTime);
        }

        [Fact]
        public async Task Handle_DtoCreatedByUserDescription_CreatedByUserDescriptionIsEqualToDto()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();
            var createdByUser = _userTestDataGenerator.Create();
            user.CreatedByUser = createdByUser;
            var createdByUserDescription = $"{user.CreatedByUser.FullName} ({user.CreatedByUser.EmailAddress})";

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.CreatedByUserDescription.Should().Be(createdByUserDescription);
        }

        [Fact]
        public async Task Handle_DtoCreatedByUserIsNull_CreatedByUserDescriptionIsEmptyOrNull()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.CreatedByUserDescription.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_UserHasOneRole_RoleShouldBeSet()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var authenticationUser = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                CVSUserId = user.Id
            };

            var role = Roles.Coach;
            List<string> roles = [role];

            _identityServiceMock.Setup(mock => mock.GetUserByCVSUserIdAsync(It.IsAny<int>())).ReturnsAsync(authenticationUser);
            _identityServiceMock.Setup(mock => mock.GetUserRolesAsync(It.IsAny<string>())).ReturnsAsync(roles);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.Role.Should().Be(role);
        }

        [Fact]
        public async Task Handle_UserHasMultipleRole_RolesShouldBeSet()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var authenticationUser = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                CVSUserId = user.Id
            };

            List<string> roles =
            [
                Roles.Administrator,
                Roles.Coach,
                Roles.Licensee,
                Roles.SystemOwner
            ];

            _identityServiceMock.Setup(mock => mock.GetUserByCVSUserIdAsync(It.IsAny<int>())).ReturnsAsync(authenticationUser);
            _identityServiceMock.Setup(mock => mock.GetUserRolesAsync(It.IsAny<string>())).ReturnsAsync(roles);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.Role.Should().Be(string.Join(", ", roles));
        }
    }
}
