using System;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence.Configurations;

namespace WorkplaceTasks.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

