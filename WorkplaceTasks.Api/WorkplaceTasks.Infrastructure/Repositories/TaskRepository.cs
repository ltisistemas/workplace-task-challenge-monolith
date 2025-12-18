using WorkplaceTasks.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence;
using InfrastructureTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;

namespace WorkplaceTasks.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TaskItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Status != InfrastructureTaskStatus.Deleted, cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR001] Failed to get task by id", ex);
        }
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TaskItems
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Status != InfrastructureTaskStatus.Deleted)
                .ToListAsync(cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR002] Failed to get all tasks", ex);
        }
    }

    public async Task<TaskItem> CreateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TaskItems.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return task;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#DBTASKERROR003] Failed to create task", ex);
        }
    }
    public async Task<TaskItem> UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        try
        {
            if (task.Status == InfrastructureTaskStatus.Deleted || task.Status == InfrastructureTaskStatus.Done) return task;
            if (string.IsNullOrEmpty(task.Title)) throw new Exception("Title is required");
            if (task.Title.Length > 200) throw new Exception("Title must be less than 200 characters");
            if (string.IsNullOrEmpty(task.Description)) throw new Exception("Description is required");

            task.UpdatedAt = DateTime.UtcNow;
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
            return task;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR004] Failed to update task", ex);
        }
    }
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var task = await _context.TaskItems.FindAsync(new object?[] { id }, cancellationToken);
            if (task is null) return false;
            if (task.Status == InfrastructureTaskStatus.Done || task.Status == InfrastructureTaskStatus.Deleted) return false;
            task.Status = InfrastructureTaskStatus.Deleted;
            task.UpdatedAt = DateTime.UtcNow;
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR005] Failed to delete task", ex);
        }
    }
}