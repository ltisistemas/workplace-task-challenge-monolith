using Microsoft.Extensions.DependencyInjection;

namespace WorkplaceTasks.Application;


public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}