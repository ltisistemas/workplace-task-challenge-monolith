using Microsoft.Extensions.DependencyInjection;
using WorkplaceTasks.Application.Tasks;
using WorkplaceTasks.Application.Interfaces;

namespace WorkplaceTasks.Application;


public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();

        return services;
    }
}