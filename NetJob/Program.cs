// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using NetJob;
using NetJob.Job;

var serviceCollection = new ServiceCollection();

JobSetup.ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

var _schedulerCenter = serviceProvider.GetRequiredService<ISchedulerCenter>();

await _schedulerCenter.AddScheduleJobAsync(new TasksQz() { Id = 1, Name = "TestJob", AssemblyName = "NetJob", ClassName = "Job.Jobs.TestJob", BeginTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddMonths(1), Cron = "0/2 * * * * ?", TriggerType = 1 });


Console.WriteLine("Hello, World!");

Console.ReadLine();
