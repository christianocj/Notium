using KnowledgeHub.Domain.Entities;

namespace KnowledgeHub.Domain.Abstrations.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        void Update(User user);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
