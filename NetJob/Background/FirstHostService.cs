using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetJob.Background;

public class FirstHostService : IHostedService
{

    private readonly ILogger<FirstHostService> _logger;

    public FirstHostService(ILogger<FirstHostService> logger)
    {
        _logger = logger;
    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FirstHostService StartAsync called.");
        Console.WriteLine("FirstHostService StartAsync called.");
        return Task.CompletedTask; 
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("FirstHostService StopAsync called.");
        Console.WriteLine("FirstHostService StopAsync called.");
        return Task.CompletedTask;
    }
}
