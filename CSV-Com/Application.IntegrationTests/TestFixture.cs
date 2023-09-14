using Infrastructure.Identity;
using Infrastructure.Persistence.CVS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests
{
    //internal class TestFixture : IDisposable
    //{
    //    public WebApplicationFactory<Program> Factory { get; init; } = null!;
    //    public IConfiguration Configuration { get; init; } = null!;
    //    public IServiceScopeFactory ScopeFactory { get; init; } = null!;
    //    public Respawner Checkpoint { get; init; } = null!;

    //    public TestFixture()
    //    {
    //        Factory = new CustomWebApplicationFactory();
    //        ScopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();
    //        Configuration = Factory.Services.GetRequiredService<IConfiguration>();

    //        Checkpoint = Respawner.CreateAsync(Configuration.GetConnectionString("DefaultConnection")!, new RespawnerOptions
    //        {
    //            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
    //        }).GetAwaiter().GetResult();
    //    }

    //    public void Dispose()
    //    {
    //        // clean up any unmanaged references
    //    }
    //}
}
