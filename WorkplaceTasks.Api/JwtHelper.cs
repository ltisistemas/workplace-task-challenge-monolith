using System.Security.Claims;
using WorkplaceTasks.Domain.Entities;

namespace WorkplaceTasks.Api;

public static class JwtHelper
{
    public static Guid GetUserId(HttpContext httpContext)
    {
        var nameIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdClaim == null || !Guid.TryParse(nameIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID não encontrado no token");
        }
        return userId;
    }
    public static RoleEnum GetUserRole(HttpContext httpContext)
    {
        var roleClaim = httpContext.User.FindFirst(ClaimTypes.Role);
        if (roleClaim == null)
        {
            throw new UnauthorizedAccessException("Role não encontrado no token");
        }

        // Converter string para RoleEnum
        return Enum.TryParse<RoleEnum>(roleClaim.Value, ignoreCase: true, out var role) 
            ? role 
            : throw new UnauthorizedAccessException($"Role inválido: {roleClaim.Value}");
    }

    public static (Guid UserId, RoleEnum Role) GetUserInfo(HttpContext httpContext)
    {
        return (GetUserId(httpContext), GetUserRole(httpContext));
    }
}