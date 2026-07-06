using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Domain.Abstrations.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default);
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    }
}
