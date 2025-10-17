using Microsoft.Extensions.DependencyInjection;
using NetJob.Job;
using Quartz;
using Quartz.Spi;
using System.Reflection;

namespace NetJob;

internal static class JobSetup
{
    public static void ConfigureServices(IServiceCollection services)
    {

        services.AddSingleton<IJobFactory, JobFactory>();
        services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
        //任务注入
        var baseType = typeof(IJob);
        var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        var referencedAssemblies = System.IO.Directory.GetFiles(path, "NetJob.dll").Select(Assembly.LoadFrom).ToArray();
        var types = referencedAssemblies
            .SelectMany(a => a.DefinedTypes)
            .Select(type => type.AsType())
            .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
        var implementTypes = types.Where(x => x.IsClass).ToArray();
        foreach (var implementType in implementTypes)
        {
            services.AddTransient(implementType);
        }
    }
}
