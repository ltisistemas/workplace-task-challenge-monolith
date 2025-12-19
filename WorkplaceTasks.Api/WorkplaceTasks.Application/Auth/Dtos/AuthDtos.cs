namespace WorkplaceTasks.Application.Auth.Dtos;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, Guid Id, string UserName, string UserEmail, string UserRole);