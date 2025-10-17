using Microsoft.Extensions.DependencyInjection;
using NetJob.Background;

namespace NetJob.ServiceExtensions;

public static class HostServiceSetup
{
    public static void ConfigureHostServices(IServiceCollection services)
    {
        services.AddHostedService<FirstHostService>();
        services.AddHostedService<FirstBackgroundServices>();
    }
}
