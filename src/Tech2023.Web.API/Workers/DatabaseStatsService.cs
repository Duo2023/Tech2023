using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;

namespace Tech2023.Web.API.Workers;

public class DatabaseStatsService : BackgroundService
{
    internal readonly ILogger<DatabaseStatsService> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _factory;
    internal readonly static TimeSpan _duration = TimeSpan.FromMinutes(5);

    public DatabaseStatsService(IServiceProvider provider)
    {
        _logger = provider.GetRequiredService<ILogger<DatabaseStatsService>>();
        _factory = provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database stats service running");

        using PeriodicTimer timer = new(_duration);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunWorkAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Database stats service stopping");
        }
    }

    internal async Task RunWorkAsync(CancellationToken token)
    {
        using var context = await _factory.CreateDbContextAsync(token);

        int userCount = await context.Users.CountAsync(token);
        int privacyPolicyCount = await context.PrivacyPolicies.CountAsync(token);

        _logger.LogInformation("{userCount} users in the database", userCount);
        _logger.LogInformation("{privacyPolicyCount} privacy policies in the database", privacyPolicyCount);
    }
}
