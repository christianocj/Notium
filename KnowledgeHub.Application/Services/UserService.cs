using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Users;
using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new KeyNotFoundException("Usuário não encontrado.");

            return MapToDto(user);
        }

        public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new KeyNotFoundException("Usuário não encontrado.");

            var existingWithEmail = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (existingWithEmail is not null && existingWithEmail.Id != userId)
                throw new InvalidOperationException("Este e-mail já está em uso.");

            user.Name = request.Name;
            user.Email = request.Email;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(ct);

            return MapToDto(user);
        }

        private static UserProfileDto MapToDto(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
