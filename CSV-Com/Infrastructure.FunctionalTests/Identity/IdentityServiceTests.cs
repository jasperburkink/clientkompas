namespace Infrastructure.FunctionalTests.Identity
{
    public class IdentityServiceIntegrationTests
    {
        //private readonly IdentityService _identityService;
        //private readonly UserManager<AuthenticationUser> _userManager;
        //private readonly SignInManager<AuthenticationUser> _signInManager;
        //private readonly IAuthorizationService _authorizationService;
        //private readonly IHasher _hasher;

        //public IdentityServiceIntegrationTests()
        //{
        //    // Setting up in-memory database and Identity-related services
        //    var serviceProvider = new ServiceCollection()
        //        .AddDbContext<DbContext>(options => options.UseInMemoryDatabase("TestDb"))
        //        .AddIdentityCore<AuthenticationUser>(options => { })
        //        .AddEntityFrameworkStores<DbContext>()
        //        .AddSignInManager()
        //        .AddAuthorization()
        //        .BuildServiceProvider();

        //    _userManager = serviceProvider.GetRequiredService<UserManager<AuthenticationUser>>();
        //    _signInManager = serviceProvider.GetRequiredService<SignInManager<AuthenticationUser>>();
        //    _authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
        //    _hasher = new Argon2Hasher(); // Using the real implementation of Argon2Hasher

        //    _identityService = new IdentityService(_userManager, _signInManager, null, _authorizationService, _hasher);
        //}

        //#region CreateUserAsync

        //[Fact]
        //public async Task CreateUserAsync_ValidInput_ShouldReturnSuccessResult()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Act
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);

        //    // Assert
        //    result.Succeeded.Should().BeTrue();
        //    userId.Should().NotBeNullOrEmpty();

        //    var user = await _userManager.FindByIdAsync(userId);
        //    user.Should().NotBeNull();
        //    user.UserName.Should().Be(userName);
        //}

        //#endregion

        //#region LoginAsync

        //[Fact]
        //public async Task LoginAsync_ValidCredentials_ShouldReturnSuccessResult()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, _) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var loginResult = await _identityService.LoginAsync(userName, password);

        //    // Assert
        //    loginResult.Succeeded.Should().BeTrue();
        //    loginResult.User.Should().NotBeNull();
        //    loginResult.User.UserName.Should().Be(userName);
        //}

        //[Fact]
        //public async Task LoginAsync_InvalidCredentials_ShouldReturnFailedResult()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "WrongPassword";

        //    // Create user
        //    var (result, _) = await _identityService.CreateUserAsync(userName, "Password123!");
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var loginResult = await _identityService.LoginAsync(userName, password);

        //    // Assert
        //    loginResult.Succeeded.Should().BeFalse();
        //    loginResult.User.Should().BeNull();
        //}

        //#endregion

        //#region GetUserNameAsync

        //[Fact]
        //public async Task GetUserNameAsync_UserExists_ShouldReturnUserName()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var result = await _identityService.GetUserNameAsync(userId);

        //    // Assert
        //    result.Should().Be(userName);
        //}

        //[Fact]
        //public async Task GetUserNameAsync_UserDoesNotExist_ShouldReturnNull()
        //{
        //    // Act
        //    var result = await _identityService.GetUserNameAsync("nonexistent");

        //    // Assert
        //    result.Should().BeNull();
        //}

        //#endregion

        //#region DeleteUserAsync

        //[Fact]
        //public async Task DeleteUserAsync_UserExists_ShouldReturnSuccessResult()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var deleteResult = await _identityService.DeleteUserAsync(userId);

        //    // Assert
        //    deleteResult.Succeeded.Should().BeTrue();

        //    var deletedUser = await _userManager.FindByIdAsync(userId);
        //    deletedUser.Should().BeNull();
        //}

        //[Fact]
        //public async Task DeleteUserAsync_UserDoesNotExist_ShouldReturnSuccessResult()
        //{
        //    // Act
        //    var result = await _identityService.DeleteUserAsync("nonexistent");

        //    // Assert
        //    result.Succeeded.Should().BeTrue();
        //}

        //#endregion

        //#region IsInRoleAsync

        //[Fact]
        //public async Task IsInRoleAsync_UserIsInRole_ShouldReturnTrue()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Assign role
        //    await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), "Admin");

        //    // Act
        //    var isInRole = await _identityService.IsInRoleAsync(userId, "Admin");

        //    // Assert
        //    isInRole.Should().BeTrue();
        //}

        //[Fact]
        //public async Task IsInRoleAsync_UserIsNotInRole_ShouldReturnFalse()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var isInRole = await _identityService.IsInRoleAsync(userId, "NonexistentRole");

        //    // Assert
        //    isInRole.Should().BeFalse();
        //}

        //#endregion
    }
}
