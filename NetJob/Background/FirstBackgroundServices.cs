using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetJob.Background;

internal class FirstBackgroundServices : BackgroundService
{

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("FirstBackgroundServices  ExecuteAsync");
        return Task.CompletedTask;
    }
}
