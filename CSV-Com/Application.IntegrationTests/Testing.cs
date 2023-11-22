namespace Application.IntegrationTests
{
    //internal partial class Testing : IClassFixture<TestFixture>
    //{
    //    private static TestFixture _fixture;
    //    private static string? _currentUserId;

    //    public Testing(TestFixture testFixture)
    //    {
    //        _fixture = testFixture;
    //    }

    //    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

    //        return await mediator.Send(request);
    //    }

    //    public static async Task SendAsync(IBaseRequest request)
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

    //        await mediator.Send(request);
    //    }

    //    public static string? GetCurrentUserId()
    //    {
    //        return _currentUserId;
    //    }

    //    public static async Task<string> RunAsDefaultUserAsync()
    //    {
    //        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    //    }

    //    public static async Task<string> RunAsAdministratorAsync()
    //    {
    //        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
    //    }

    //    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    //        var user = new ApplicationUser { UserName = userName, Email = userName };

    //        var result = await userManager.CreateAsync(user, password);

    //        if (roles.Any())
    //        {
    //            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //            foreach (var role in roles)
    //            {
    //                await roleManager.CreateAsync(new IdentityRole(role));
    //            }

    //            await userManager.AddToRolesAsync(user, roles);
    //        }

    //        if (result.Succeeded)
    //        {
    //            _currentUserId = user.Id;

    //            return _currentUserId;
    //        }

    //        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

    //        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    //    }

    //    public static async Task ResetState()
    //    {
    //        try
    //        {
    //            await _fixture.Checkpoint.ResetAsync(_fixture.Configuration.GetConnectionString("AuthenticationConnectionString")!);
    //            //await _fixture.Checkpoint.ResetAsync(_fixture.Configuration.GetConnectionString("DefaultConnection")!);
    //        }
    //        catch (Exception)
    //        {
    //        }

    //        _currentUserId = null;
    //    }

    //    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
    //        where TEntity : class
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

    //        return await context.FindAsync<TEntity>(keyValues);
    //    }

    //    public static async Task AddAsync<TEntity>(TEntity entity)
    //        where TEntity : class
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

    //        context.Add(entity);

    //        await context.SaveChangesAsync();
    //    }

    //    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    //    {
    //        using var scope = _fixture.ScopeFactory.CreateScope();

    //        var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

    //        return await context.Set<TEntity>().CountAsync();
    //    }
    //}
}
