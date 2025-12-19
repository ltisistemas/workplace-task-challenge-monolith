using WorkplaceTasks.Application.Interfaces;
using WorkplaceTasks.Application.Users.Dtos;
using WorkplaceTasks.Domain.Entities;
using DomainRoleEnum = WorkplaceTasks.Domain.Entities.RoleEnum;
using PasswordHasher = WorkplaceTasks.Application.Services.PasswordHasherService;

namespace WorkplaceTasks.Application.Users;

public class UserTaskService(IUserTaskRepository repository) : IUserTaskService
{
    public async Task<UserTaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await repository.GetByIdAsync(id, cancellationToken);
            return entity == null 
                ? throw new Exception("[#USERTASKERROR001] Failed to get user by id") 
                : MapToResponse((entity));
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR001] Failed to get user by id", ex);
        }
    }

    public async Task<IEnumerable<UserTaskResponse>> GetAllAsync(Guid loggedUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await repository.GetByIdAsync(loggedUserId, cancellationToken); 
            if (user is null) throw new Exception("[#USERTASKERROR002] User not found");

            if (user.Role != RoleEnum.Admin) throw new Exception("[#USERTASKERROR002] User is not authorized to get all users");
            
            var entities = await repository.GetAllAsync(cancellationToken);
            return entities.Select(MapToResponse);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR002] Failed to get all users", ex);
        }
    }

    public async Task<UserTaskResponse> CreateAsync(CreateUserTaskIRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserName)) throw new Exception("[#USERTASKERROR003] Username is required");
            if (string.IsNullOrEmpty(request.UserEmail)) throw new Exception("[#USERTASKERROR003] User email is required");
            if (string.IsNullOrEmpty(request.UserPassword)) throw new Exception("[#USERTASKERROR003] User password is required");

            var hashedPassword = PasswordHasher.Hash(request.UserPassword);

            var entity = new UserTask
            {
                Username = request.UserName,
                UserEmail = request.UserEmail,
                Role = request.Role,
                UserPassword = hashedPassword,
            };

            var createdEntity = await repository.CreateAsync(entity, cancellationToken);
            return MapToResponse(createdEntity);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR003] Failed to create user", ex);
        }
    }

    public async Task<UserTaskResponse?> UpdateAsync(UpdateUserTaskIRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await repository.GetByIdAsync(request.UserId, cancellationToken);
            if (entity is null) throw new Exception("[#USERTASKERROR006] User not found");

            if (entity.Id != request.UserId && entity.Role != RoleEnum.Admin) throw new Exception("[#USERTASKERROR006] User is not the logged user or an admin");

            entity.Username = request.UserName;
            entity.Role = (DomainRoleEnum) request.Role;
            entity.UpdatedAt = DateTime.UtcNow;
            
            return MapToResponse(await repository.UpdateAsync(entity, cancellationToken));
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR006] Failed to update user", ex);
        }
    }

    public async Task<UserTaskResponse?> UpdatePasswordAsync(UpdatePasswordUserTaskIRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var userLogged = await repository.GetByIdAsync(request.LoggedUserId, cancellationToken);
            if (userLogged is null) throw new Exception("[#USERTASKERROR006] User not found");

            if (request.UserId != request.LoggedUserId && userLogged.Role != DomainRoleEnum.Admin) throw new Exception("[#USERTASKERROR006] User is not the logged user or an admin");

            var hashedPassword = PasswordHasher.Hash(request.UserPassword);

            userLogged.UserPassword = hashedPassword;
            userLogged.UpdatedAt = DateTime.UtcNow;

            var updatedEntity = await repository.UpdateAsync(userLogged, cancellationToken);
            return MapToResponse(updatedEntity);
        }   
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR006] Failed to update password", ex);
        }
    }

    public async Task<bool> DeleteAsync(DeleteUserTaskIRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await repository.GetByIdAsync(request.UserId, cancellationToken);
            if (entity is null) throw new Exception("[#USERTASKERROR007] User not found");

            if (entity.Role != DomainRoleEnum.Admin) throw new Exception("[#USERTASKERROR007] User is not the logged user or an admin");

            return await repository.DeleteAsync(entity.Id, cancellationToken);
        }
        catch (System.Exception ex)
        {
            throw new Exception("[#USERTASKERROR007] Failed to delete user", ex);
        }
    }

    private UserTaskResponse MapToResponse(UserTask entity)
    {
        return new UserTaskResponse(
            entity.Id, 
            entity.Username, 
            entity.UserEmail, 
            entity.Role, 
            entity.CreatedAt, 
            entity.UpdatedAt,
            entity.DeletedAt ?? null
        );
    }
}