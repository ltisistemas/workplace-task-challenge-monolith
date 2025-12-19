using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Application.Services;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence;

namespace WorkplaceTasks.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedUsersAsync(AppDbContext context)
    {
        // Verifica se já existem usuários no banco
        var hasUsers = await context.UserTasks.AnyAsync();
        
        if (hasUsers)
        {
            Console.WriteLine("Usuários já existem no banco de dados. Seed não será executado.");
            return;
        }

        Console.WriteLine("Iniciando seed de usuários...");

        var users = new List<UserTask>
        {
            new UserTask
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                UserEmail = "admin@bimworkplace.com",
                UserPassword = PasswordHasherService.Hash("admin123"),
                Role = RoleEnum.Admin,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new UserTask
            {
                Id = Guid.NewGuid(),
                Username = "manager",
                UserEmail = "manager@bimworkplace.com",
                UserPassword = PasswordHasherService.Hash("manager123"),
                Role = RoleEnum.Manager,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new UserTask
            {
                Id = Guid.NewGuid(),
                Username = "member",
                UserEmail = "member@bimworkplace.com",
                UserPassword = PasswordHasherService.Hash("member123"),
                Role = RoleEnum.Member,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.UserTasks.AddRangeAsync(users);
        await context.SaveChangesAsync();

        Console.WriteLine($"Seed concluído! {users.Count} usuários criados.");
    }
}

