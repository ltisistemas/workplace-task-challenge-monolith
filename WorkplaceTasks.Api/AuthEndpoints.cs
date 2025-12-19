using WorkplaceTasks.Application.Auth;
using WorkplaceTasks.Application.Auth.Dtos;

namespace WorkplaceTasks.Api;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", async (LoginRequest req, IAuthService authService, CancellationToken cancellationToken) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
                        return Results.BadRequest("Email e senha são obrigatórios");

                    var result = await authService.LoginAsync(new LoginRequest(req.Email, req.Password), cancellationToken);
                
                    return result == null ? Results.Unauthorized() : Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("Login")
            .WithSummary("Realizar login")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();

        return app;
    }
}