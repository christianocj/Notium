using KnowledgeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Domain.Abstrations.Repository
{
    public interface ITagRepository
    {
        Task<Tag?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Tag?> GetByNameAsync(Guid userId, string name, CancellationToken ct = default);
        Task<IEnumerable<Tag>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task AddAsync(Tag tag, CancellationToken ct = default);
        void Remove(Tag tag);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
