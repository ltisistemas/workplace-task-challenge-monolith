using WorkplaceTasks.Domain.Entities;

namespace WorkplaceTasks.Application.Interfaces;

public interface IUserTaskRepository
{
    Task<IEnumerable<UserTask>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserTask?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserTask> CreateAsync(UserTask userTask, CancellationToken cancellationToken = default);
    Task<UserTask> UpdateAsync(UserTask userTask, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}