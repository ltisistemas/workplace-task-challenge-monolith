using WorkplaceTasks.Application.Tasks.Dtos;

namespace WorkplaceTasks.Application.Tasks;

public interface ITaskService
{
    Task<TaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskResponse> CreateAsync(CreateTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<TaskResponse?> UpdateAsync(UpdateTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default);
    Task<TaskResponse> DeleteAsync(DeleteTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default);
}