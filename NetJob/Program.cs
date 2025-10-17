// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetJob;
using NetJob.Job;
using NetJob.ServiceExtensions;

var builder = Host.CreateDefaultBuilder(args)
  .ConfigureServices(services =>
{
    JobSetup.ConfigureServices(services);
    HostServiceSetup.ConfigureHostServices(services);
}).ConfigureLogging(logger =>
{
    logger.SetMinimumLevel(LogLevel.Information);
}).Build();

var _schedulerCenter = builder.Services.GetRequiredService<ISchedulerCenter>();

await _schedulerCenter.AddScheduleJobAsync(new TasksQz() { Id = 1, Name = "TestJob", AssemblyName = "NetJob", ClassName = "Job.Jobs.TestJob", BeginTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddMonths(1), Cron = "0/2 * * * * ?", TriggerType = 1, IsStart = true });

Console.WriteLine("Hello, World!");

 await builder.RunAsync();



