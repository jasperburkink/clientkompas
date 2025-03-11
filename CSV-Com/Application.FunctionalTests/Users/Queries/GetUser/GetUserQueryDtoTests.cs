using Application.Common.Mappings;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.User;

namespace Application.FunctionalTests.Users.Queries.GetUser
{
    public class GetUserQueryDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<User> _userTestDataGenerator;
        private IMapper _mapper;

        [SetUp]
        public async Task Initialize()
        {
            _userTestDataGenerator = new UserDataGenerator();

            var configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = configuration.CreateMapper();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnSuccessResult()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().BeEquivalentTo(userDto);
        }

        [Test]
        public async Task Handle_UserDoesNotExists_DtoShouldBeNull()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().BeNull();
        }

        [Test]
        public async Task Handle_DtoFirstName_FirstNameIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.FirstName.Should().Be(user.FirstName);
        }

        [Test]
        public async Task Handle_DtoPrefixLastName_PrefixLastNameIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            user.PrefixLastName = "van der";
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.PrefixLastName.Should().Be(user.PrefixLastName);
        }

        [Test]
        public async Task Handle_DtoLastName_LastNameIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.LastName.Should().Be(user.LastName);
        }

        [Test]
        public async Task Handle_DtoTelephoneNumber_TelephoneNumberIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.TelephoneNumber.Should().Be(user.TelephoneNumber);
        }

        [Test]
        public async Task Handle_DtoFullName_FullNameIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.FullName.Should().Be(user.FullName);
        }

        [Test]
        public async Task Handle_DtoEmailAddress_EmailAddressIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.EmailAddress.Should().Be(user.EmailAddress);
        }

        [Test]
        public async Task Handle_DtoDeactivationDateTime_DeactivationDateTimeIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.DeactivationDateTime.Should().Be(user.DeactivationDateTime);
        }

        [Test]
        public async Task Handle_DtoCreatedByUserDescription_CreatedByUserDescriptionIsEqualToDto()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();

            var createdByUser = _userTestDataGenerator.Create();
            await AddAsync(createdByUser);

            user.CreatedByUserId = createdByUser.Id;
            var createdByUserDescription = $"{createdByUser.FullName} ({createdByUser.EmailAddress})";

            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.CreatedByUserDescription.Should().Be(createdByUserDescription);
        }

        [Test]
        public async Task Handle_DtoCreatedByUserIsNull_CreatedByUserDescriptionIsEmptyOrNull()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.CreatedByUserDescription.Should().BeNullOrEmpty();
        }
    }
}
