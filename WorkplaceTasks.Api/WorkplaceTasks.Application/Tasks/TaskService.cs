using WorkplaceTasks.Application.Tasks;
using WorkplaceTasks.Application.Tasks.Dtos;
using WorkplaceTasks.Domain.Entities;
using InfrastructureTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;
using WorkplaceTasks.Application.Interfaces;

namespace WorkplaceTasks.Application.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
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
            // Check error from repository
            if (ex.Message.Contains("[#TASKERROR001]")) throw new Exception(ex.Message, ex);

            throw new Exception("[#TASKERROR006] Failed to get task by id", ex);
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
            // Check error from repository
            if (ex.Message.Contains("[#TASKERROR002]")) throw new Exception(ex.Message, ex);

            throw new Exception("[#TASKERROR007] Failed to get all tasks", ex);
        }
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
            };

            var createdTask = await _repository.CreateAsync(entity, cancellationToken);

            return MapToResponse(createdTask);
        }
        catch (System.Exception ex)
        {
            // Check error from repository
            if (ex.Message.Contains("[#DBTASKERROR003]")) throw new Exception(ex.Message, ex);

            throw new Exception("[#TASKERROR008] Failed to create task", ex);
        }
    }
    public async Task<TaskResponse?> UpdateAsync(UpdateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null) throw new Exception("Task not found");

            entity.Title = request.Title.Trim();
            entity.Description = request.Description.Trim() ?? string.Empty;
            entity.Status = (InfrastructureTaskStatus)request.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            return MapToResponse(await _repository.UpdateAsync(entity, cancellationToken));
        }
        catch (System.Exception ex)
        {
            // Check error from repository
            if (ex.Message.Contains("[#TASKERROR004]")) throw new Exception(ex.Message, ex);

            throw new Exception("[#TASKERROR009] Failed to update task", ex);
        }
    }
    public async Task<TaskResponse> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(Id, cancellationToken) ?? throw new Exception("Task not found");

            await _repository.DeleteAsync(entity.Id, cancellationToken);
            return MapToResponse(entity);
        }
        catch (System.Exception ex)
        {
            // Check error from repository
            if (ex.Message.Contains("[#TASKERROR005]")) throw new Exception(ex.Message, ex);

            throw new Exception("[#TASKERROR010] Failed to delete task", ex);
        }
    }
    private static TaskResponse MapToResponse(TaskItem entity)
    {
        return new TaskResponse(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Status,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}