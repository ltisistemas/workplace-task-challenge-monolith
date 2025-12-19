using BCrypt.Net;

namespace WorkplaceTasks.Application.Services;

public static class PasswordHasherService
{
    private const int DefaultSaltRounds = 12;

    // Gerar hash da senha
    public static string Hash(string value, int saltRounds = DefaultSaltRounds)
    {
        return BCrypt.Net.BCrypt.HashPassword(value, BCrypt.Net.BCrypt.GenerateSalt(saltRounds));
    }

    // Verificar se a senha corresponde ao hash
    public static bool Verify(string valueAttempt, string hashedValue)
    {
        return BCrypt.Net.BCrypt.Verify(valueAttempt, hashedValue);
    }
}