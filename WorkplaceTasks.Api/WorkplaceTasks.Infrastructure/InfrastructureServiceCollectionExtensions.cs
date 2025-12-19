using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkplaceTasks.Application.Interfaces;
using WorkplaceTasks.Infrastructure.Repositories;
using WorkplaceTasks.Infrastructure.Persistence;
using WorkplaceTasks.Domain.Entities;

namespace WorkplaceTasks.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserTaskRepository, UserTaskRepository>();

        return services;
    }
}