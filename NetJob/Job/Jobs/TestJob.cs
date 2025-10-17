using Microsoft.Extensions.Logging;
using NetJob.Background;
using Quartz;

namespace NetJob.Job.Jobs;

public class TestJob : JobBase
{
    private readonly ILogger<FirstHostService> _logger;

    public TestJob(ILogger<FirstHostService> logger)
    {
        _logger = logger;
    }

    public override Task Run(IJobExecutionContext context)
    {

        Console.WriteLine("Hello, World! Hello First Job");

        _logger.LogInformation("Hello, World! Hello First Job");

        return Task.FromResult(true);
    }
}
