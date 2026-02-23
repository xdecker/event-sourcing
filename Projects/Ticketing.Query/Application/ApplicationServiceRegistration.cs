using System;
using System.Reflection;

namespace Ticketing.Query.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetEntryAssembly();
        services.AddMediatR(m =>
        {
            m.RegisterServicesFromAssemblies(currentAssembly!);
        });

        return services;
    }
}
