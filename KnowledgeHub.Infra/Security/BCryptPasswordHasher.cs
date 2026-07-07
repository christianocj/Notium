using KnowledgeHub.Application.Abstrations.Security;

namespace KnowledgeHub.Infra.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string passwordDigitada, string hashArmazenado)
        {
            return BCrypt.Net.BCrypt.Verify(passwordDigitada, hashArmazenado);
        }
    }
}
