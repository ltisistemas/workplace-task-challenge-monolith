using WorkplaceTasks.Application.Users.Dtos;

namespace WorkplaceTasks.Application.Users;

public interface IUserTaskService
{
    Task<UserTaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserTaskResponse>> GetAllAsync(Guid loggedUserId, CancellationToken cancellationToken = default);
    Task<UserTaskResponse> CreateAsync(CreateUserTaskIRequest request, CancellationToken cancellationToken = default);
    Task<UserTaskResponse?> UpdateAsync(UpdateUserTaskIRequest request, CancellationToken cancellationToken = default);
    Task<UserTaskResponse?> UpdatePasswordAsync(UpdatePasswordUserTaskIRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(DeleteUserTaskIRequest request, CancellationToken cancellationToken = default);
}