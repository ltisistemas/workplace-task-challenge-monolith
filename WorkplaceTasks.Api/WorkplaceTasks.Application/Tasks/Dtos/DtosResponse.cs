using WorkplaceTasks.Domain.Entities;
using DomainTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;

namespace WorkplaceTasks.Application.Tasks.Dtos;

public record CreateTaskItemRequest(String Title, String Description);

public record UpdateTaskItemRequest(
    Guid Id, 
    String Title, 
    String Description, 
    DomainTaskStatus Status
);

public record TaskResponse(
    Guid Id,
    String Title,
    String Description,
    DomainTaskStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);