using System;

namespace KnowledgeHub.Infra.Security
{
    public class BCryptPasswordHasherIPasswordHasher
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
