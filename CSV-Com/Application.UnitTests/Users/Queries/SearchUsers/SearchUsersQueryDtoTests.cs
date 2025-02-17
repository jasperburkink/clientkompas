using Application.Common.Mappings;
using Application.Users.Queries.SearchUsers;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.UnitTests.Users.Queries.SearchUsers
{
    public class SearchUsersQueryDtoTests
    {
        private readonly IMapper _mapper;

        public SearchUsersQueryDtoTests()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>()); // Zorg ervoor dat MappingProfile correct is geconfigureerd
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Mapping_UserToDto_ShouldMapCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                PrefixLastName = "van",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "123456789",
                IsDeactivated = false
            };

            // Act
            var dto = _mapper.Map<SearchUsersQueryDto>(user);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(user.Id);
            dto.FirstName.Should().Be(user.FirstName);
            dto.PrefixLastName.Should().Be(user.PrefixLastName);
            dto.LastName.Should().Be(user.LastName);
            dto.FullName.Should().Be("John van Doe"); // FullName moet correct worden gegenereerd
            dto.IsDeactivated.Should().Be(user.IsDeactivated);
        }

        [Fact]
        public void Mapping_UserWithNoPrefixLastName_ShouldStillMapCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 2,
                FirstName = "Alice",
                PrefixLastName = null,
                LastName = "Smith",
                EmailAddress = "alice@example.com",
                TelephoneNumber = "987654321",
                IsDeactivated = true
            };

            // Act
            var dto = _mapper.Map<SearchUsersQueryDto>(user);

            // Assert
            dto.Should().NotBeNull();
            dto.FirstName.Should().Be("Alice");
            dto.PrefixLastName.Should().BeNull(); // Geen PrefixLastName, dus null
            dto.LastName.Should().Be("Smith");
            dto.FullName.Should().Be("Alice Smith"); // Geen extra spaties
            dto.IsDeactivated.Should().BeTrue();
        }

        [Fact]
        public void Mapping_UserWithMultipleSpaces_ShouldFormatFullNameCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 3,
                FirstName = "  Bob ",
                PrefixLastName = " van  der ",
                LastName = " Berg ",
                EmailAddress = "bob@example.com",
                TelephoneNumber = "555444333",
                IsDeactivated = false
            };

            // Act
            var dto = _mapper.Map<SearchUsersQueryDto>(user);

            // Assert
            dto.FullName.Should().Be("Bob van der Berg"); // Extra spaties moeten worden verwijderd
        }

        [Fact]
        public void Mapping_UserWithOnlyFirstAndLastName_ShouldStillHaveValidFullName()
        {
            // Arrange
            var user = new User
            {
                Id = 4,
                FirstName = "Emma",
                PrefixLastName = "",
                LastName = "Brown",
                EmailAddress = "emma@example.com",
                TelephoneNumber = "666777888",
                IsDeactivated = false
            };

            // Act
            var dto = _mapper.Map<SearchUsersQueryDto>(user);

            // Assert
            dto.FullName.Should().Be("Emma Brown"); // Geen lege prefix in FullName
        }
    }
}
