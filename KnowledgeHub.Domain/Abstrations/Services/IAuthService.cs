using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Domain.Abstrations.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
