using System.Reflection;
using MongoDB.Driver;

namespace Ticketing.Command.Features.Apis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMinimalApis(
        this IServiceCollection services
    )
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var minimalApis = currentAssembly.GetTypes()
        .Where(ass => typeof(IMinimalApi).IsAssignableFrom(ass) &&
                ass != typeof(IMinimalApi) &&
                ass.IsPublic && !ass.IsAbstract
        );

        foreach (var api in minimalApis)
        {
            services.AddSingleton(typeof(IMinimalApi), api);
        }

        return services;
    }
}