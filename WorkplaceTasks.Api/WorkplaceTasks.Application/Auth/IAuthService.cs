using WorkplaceTasks.Application.Auth.Dtos;
using LoginUserRequest = WorkplaceTasks.Application.Auth.Dtos.LoginRequest;

namespace WorkplaceTasks.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
    
    string GenerateToken(Guid userId, string email, string role);
}