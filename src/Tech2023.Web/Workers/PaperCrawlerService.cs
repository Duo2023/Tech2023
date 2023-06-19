using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Tech2023.DAL;

namespace Tech2023.Web.Workers;

/// <summary>
/// Background service that retrieves exam papers and sorts filters to add them to the database
/// </summary>
public class PaperCrawlerService : BackgroundService
{
    internal readonly ILogger<PaperCrawlerService> _logger;
    internal readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    internal readonly IOptions<PaperCrawlerOptions> _options;

    public PaperCrawlerService(ILogger<PaperCrawlerService> logger, IDbContextFactory<ApplicationDbContext> contextFactory, IOptions<PaperCrawlerOptions> options)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(default);

        if (!_options.Value.Enable)
        {
            return;
        }

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                if (!_options.Value.Enable)
                {
                    return;
                }

                await InternalExecuteAsync();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("The paper retrieval service");
        }
    }

    internal async Task InternalExecuteAsync()
    {
        await Task.CompletedTask;
    }
}
