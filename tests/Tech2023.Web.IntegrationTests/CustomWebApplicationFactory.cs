using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Tech2023.DAL;
using Tech2023.Web.Shared.Email;

namespace Tech2023.Web.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveService<IConfigureOptions<EmailOptions>>();

            services.RemoveDbContextFactory<ApplicationDbContext>();

            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();

                options.UseInMemoryDatabase($"{Assembly.GetExecutingAssembly().FullName}");
            });

            services.Configure<EmailOptions>(options =>
            {
                options.SenderName = "Tests";
                options.SmtpServer = "test-smtp.example.com";
                options.Port = 587;
                options.FromEmail = "test@test.com";
                options.Username = options.FromEmail;
                options.Password = "*********";
            });
        });
    }

}


internal static class ServiceExtensions
{
    public static IServiceCollection RemoveDbContextFactory<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        // TODO: we have to override this error because of the way AddDbContextFactory is designed, look into workarounds
#pragma warning disable EF1001 // Internal EF Core API usage.
        return services.RemoveService<IDbContextFactory<TDbContext>>()
            .RemoveService<TDbContext>()
            .RemoveService<IDbContextFactorySource<TDbContext>>()
            .RemoveService<DbContextOptions<TDbContext>>();
#pragma warning restore EF1001 // Internal EF Core API usage.
    }

    public static IServiceCollection RemoveService<TService>(this IServiceCollection services)
    {
        var service = services.FirstOrDefault(descriptor =>
            descriptor.ServiceType == typeof(TService));

        if (service != null)
            services.Remove(service);

        return services;
    }
}
