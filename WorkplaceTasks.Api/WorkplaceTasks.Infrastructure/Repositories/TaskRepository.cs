using WorkplaceTasks.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence;
using InfrastructureTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;

namespace WorkplaceTasks.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.TaskItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Status != InfrastructureTaskStatus.Deleted, cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR017] Failed to get task by id", ex);
        }
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.TaskItems
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Status != InfrastructureTaskStatus.Deleted)
                .ToListAsync(cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR018] Failed to get all tasks", ex);
        }
    }

    public async Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.TaskItems.AddAsync(task, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return task;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR019] Failed to create task", ex);
        }
    }
    public async Task<TaskItem> UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            context.TaskItems.Update(task);
            await context.SaveChangesAsync(cancellationToken);

            return task;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR020] Failed to update task", ex);
        }
    }
    public async Task<bool> DeleteAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            // Soft delete the task
            task.Status = InfrastructureTaskStatus.Deleted;
            task.UpdatedAt = DateTime.UtcNow;
            context.TaskItems.Update(task);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR021] Failed to delete task", ex);
        }
    }
}