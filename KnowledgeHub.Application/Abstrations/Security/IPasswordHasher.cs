using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.Abstrations.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string passwordDigitada, string hashArmazenado);
    }
}
