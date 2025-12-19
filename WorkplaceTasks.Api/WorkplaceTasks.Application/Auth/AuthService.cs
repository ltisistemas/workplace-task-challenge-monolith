using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkplaceTasks.Application.Auth.Dtos;
using WorkplaceTasks.Application.Interfaces;
using LoginUserRequest = WorkplaceTasks.Application.Auth.Dtos.LoginRequest;
using PasswordHasher = WorkplaceTasks.Application.Services.PasswordHasherService;

namespace WorkplaceTasks.Application.Auth;

public class AuthService(IUserTaskRepository repository, IConfiguration configuration): IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginUserRequest req, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await repository.GetByEmailAsync(req.Email, cancellationToken);
            if (user == null) return null;

            var hahed = PasswordHasher.Hash(req.Password);

            if (PasswordHasher.Verify(hahed, user.UserPassword))
                return null;

            var token = GenerateToken(user.Id, user.UserEmail, user.Role.ToString());

            return new LoginResponse(token, user.Id, user.Username, user.UserEmail,  user.Role.ToString());
        }
        catch (Exception e)
        {
            throw new Exception("[401] Login not found", e);
        }
    }

    public string GenerateToken(Guid userId, string email, string role)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var expiresInMinutes = int.Parse(configuration["Jwt:ExpiresInMinutes"] ?? "60");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}