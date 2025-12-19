using WorkplaceTasks.Application.Interfaces;
using WorkplaceTasks.Domain.Entities;
using WorkplaceTasks.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WorkplaceTasks.Infrastructure.Repositories;

public class UserTaskRepository(AppDbContext context) : IUserTaskRepository
{
    public async Task<IEnumerable<UserTask>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.UserTasks.AsNoTracking().Where(x => x.DeletedAt == null).ToListAsync(cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR001] Failed to get all users", ex);
        }
    }

    public async Task<UserTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.UserTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR002] Failed to get user by email", ex);
        }
    }
    public async Task<UserTask?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.UserTasks.AsNoTracking().FirstOrDefaultAsync(x => x.UserEmail == email, cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR002] Failed to get user by email", ex);
        }
    }

    public async Task<UserTask> CreateAsync(UserTask userTask, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.UserTasks.AddAsync(userTask, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return userTask;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR003] Failed to create user", ex);
        }
    }

    public async Task<UserTask> UpdateAsync(UserTask userTask, CancellationToken cancellationToken = default)
    {
        try
        {
            context.UserTasks.Update(userTask);
            await context.SaveChangesAsync(cancellationToken);
            return userTask;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR004] Failed to update user", ex);
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await context.UserTasks.FindAsync(new object?[] { id }, cancellationToken);
            if (user is null) throw new Exception("[#USERTASKERROR005] User not found");
            user.DeletedAt = DateTime.UtcNow;
            context.UserTasks.Update(user);
            await context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR006] Failed to delete user", ex);
        }
    }
}