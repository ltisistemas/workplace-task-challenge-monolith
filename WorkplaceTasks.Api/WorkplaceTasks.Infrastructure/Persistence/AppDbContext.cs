using System;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence.Configurations;

namespace WorkplaceTasks.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<UserTask> UserTasks => Set<UserTask>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
        modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}

