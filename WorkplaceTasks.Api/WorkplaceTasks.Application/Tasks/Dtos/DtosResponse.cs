using WorkplaceTasks.Domain.Entities;
using DomainTaskStatus = WorkplaceTasks.Domain.Entities.TaskStatus;

namespace WorkplaceTasks.Application.Tasks.Dtos;

public record CreateTaskItemRequest(
    string Title, 
    string Description
);

public record UpdateTaskItemRequest(
    Guid Id, 
    string Title, 
    string Description, 
    DomainTaskStatus Status,
    Guid TaskUserId
);

public record DeleteTaskItemRequest(
    Guid TaskId,
    Guid TaskUserId
);

public record TaskResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string Description,
    DomainTaskStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);