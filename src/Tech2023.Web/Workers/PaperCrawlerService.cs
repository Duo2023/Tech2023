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
    internal readonly IHttpClientFactory _httpFactory;

    public PaperCrawlerService(ILogger<PaperCrawlerService> logger, IDbContextFactory<ApplicationDbContext> contextFactory, IOptions<PaperCrawlerOptions> options, IHttpClientFactory httpFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _options = options;
        _httpFactory = httpFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_options.Value.Frequency));

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

                await InternalExecuteAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("The paper retrieval service");
        }
    }

    internal async Task InternalExecuteAsync(CancellationToken cancellationToken)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var httpClient = _httpFactory.CreateClient(Clients.Crawler);


        
    }
}
