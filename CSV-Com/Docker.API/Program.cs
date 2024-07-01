
using Docker.API.Exceptions;
using Docker.API.Options;
using Docker.API.Services;

namespace Docker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IDockerRefreshService, DockerRefreshService>();
            builder.Services.AddSingleton<IDelayedKillService, DelayedKillService>();
            builder.Services.AddSingleton<IDockerInfoService, DockerInfoService>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.Configure<SecurityOptions>(
                builder.Configuration.GetSection(SecurityOptions.Position));

            var app = builder.Build();

            // Use exception handling
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
