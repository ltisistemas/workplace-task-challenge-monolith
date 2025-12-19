using Microsoft.Extensions.DependencyInjection;
using WorkplaceTasks.Application.Auth;
using WorkplaceTasks.Application.Tasks;
using WorkplaceTasks.Application.Users;

namespace WorkplaceTasks.Application;


public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserTaskService, UserTaskService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}