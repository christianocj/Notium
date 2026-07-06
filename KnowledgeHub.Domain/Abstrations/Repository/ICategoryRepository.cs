using KnowledgeHub.Domain.Entities;

namespace KnowledgeHub.Domain.Abstrations.Repository
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task AddAsync(Category category, CancellationToken ct = default);
        void Update(Category category);
        void Remove(Category category);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
