namespace Ticketing.Command.Features.Apis;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapMinimalApisEndpoints(
        this IEndpointRouteBuilder endpointRouteBuilder
    )
    {
        var minimalApis = endpointRouteBuilder.ServiceProvider.GetServices<IMinimalApi>();

        foreach (IMinimalApi api in minimalApis)
        {
            api.AddEndpoint(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}