using WorkplaceTasks.Application.Tasks;
using WorkplaceTasks.Application.Tasks.Dtos;
using WorkplaceTasks.Domain.Entities;
using InfrastructureTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;
using WorkplaceTasks.Application.Interfaces;

namespace WorkplaceTasks.Application.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IUserTaskRepository _userTaskRepository;

    public TaskService(ITaskRepository repository, IUserTaskRepository userTaskRepository)
    {
        _repository = repository;
        _userTaskRepository = userTaskRepository;
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            return entity is null ? null : MapToResponse(entity);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR001] Failed to get task by id", ex);
        }
    }

    public async Task<IEnumerable<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await _repository.GetAllAsync(cancellationToken);

            return entities.Select(MapToResponse);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR002] Failed to get all tasks", ex);
        }
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Title)) throw new Exception("[#TASKERROR003] Title is required");
            if (string.IsNullOrEmpty(request.Description)) throw new Exception("[#TASKERROR004] Description is required");

            var entity = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                UserId = userId,
            };

            var createdTask = await _repository.CreateAsync(entity, cancellationToken);

            return MapToResponse(createdTask);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR005] Failed to create task", ex);
        }
    }
    public async Task<TaskResponse?> UpdateAsync(UpdateTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if task exists
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) throw new Exception("[#TASKERROR006] Task not found");

            // Check if user logged exists
            var user = await _userTaskRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null) throw new Exception("[#TASKERROR007] User logged not found");

            // Check if user has permission to update the task
            if (user.Id != entity.UserId && user.Role == RoleEnum.Member)
            {
                throw new Exception("[#TASKERROR008] You are not allowed to update this task");
            }

            // Check if title is required
            if (string.IsNullOrEmpty(request.Title)) throw new Exception("[#TASKERROR009] Title is required");  
            // Check if title is less than 200 characters
            if (request.Title.Length > 200) throw new Exception("[#TASKERROR010] Title must be less than 200 characters");
            // Check if description is required
            if (string.IsNullOrEmpty(request.Description)) throw new Exception("[#TASKERROR011] Description is required");
            // Check if task is already deleted or done
            if (entity.Status == InfrastructureTaskStatus.Deleted || entity.Status == InfrastructureTaskStatus.Done) return MapToResponse(entity);
            
            // Update task
            entity.Title = request.Title.Trim();
            entity.Description = request.Description.Trim() ?? string.Empty;
            entity.Status = (InfrastructureTaskStatus)request.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            return MapToResponse(await _repository.UpdateAsync(entity, cancellationToken));
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR012] Failed to update task", ex);
        }
    }
    public async Task<TaskResponse> DeleteAsync(DeleteTaskItemRequest request, Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.TaskId, cancellationToken);
            if (entity is null) throw new Exception("[#TASKERROR013] Task not found");

            // Check if task is already deleted or done
            if (entity.Status == InfrastructureTaskStatus.Done || entity.Status == InfrastructureTaskStatus.Deleted) return MapToResponse(entity);

            // Check if user logged exists
            var userLogged = await _userTaskRepository.GetByIdAsync(userId, cancellationToken);
            if (userLogged is null) throw new Exception("[#TASKERROR014] User logged not found");

            // Check if user has permission to update the task
            if (userLogged.Id != entity.UserId && (userLogged.Role == RoleEnum.Member || userLogged.Role == RoleEnum.Manager))
            {
                throw new Exception("[#TASKERROR015] You are not allowed to update this task");
            }

            await _repository.DeleteAsync(entity, cancellationToken);
            return MapToResponse(entity);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#TASKERROR016] Failed to delete task", ex);
        }
    }
    private static TaskResponse MapToResponse(TaskItem entity)
    {
        return new TaskResponse(
            entity.Id,
            entity.UserId,
            entity.Title,
            entity.Description,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}