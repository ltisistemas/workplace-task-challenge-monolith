using WorkplaceTasks.Domain.Entities;

namespace WorkplaceTasks.Application.Users.Dtos;

public record CreateUserTaskIRequest(
    string UserName, 
    string UserEmail,
    string UserPassword,
    RoleEnum Role
)
;

public record UpdateUserTaskIRequest(
    Guid UserId,
    string UserName,
    RoleEnum Role
);

public record UpdatePasswordUserTaskIRequest(
    Guid UserId,
    Guid LoggedUserId,
    string UserPassword
);

public record DeleteUserTaskIRequest(
    Guid UserId,
    Guid LoggedUserId
);

public record UserTaskResponse(
    Guid UserId,
    string UserName,
    string UserEmail,
    RoleEnum Role,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt
);