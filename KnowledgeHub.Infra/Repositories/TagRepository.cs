using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Infra.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;
        public TagRepository(AppDbContext context) => _context = context;

        public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Tags.FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<Tag?> GetByNameAsync(Guid userId, string name, CancellationToken ct = default)
            => await _context.Tags.FirstOrDefaultAsync(t => t.UserId == userId && t.Name == name, ct);

        public async Task<IEnumerable<Tag>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => await _context.Tags.Where(t => t.UserId == userId).ToListAsync(ct);

        public async Task AddAsync(Tag tag, CancellationToken ct = default)
            => await _context.Tags.AddAsync(tag, ct);

        public void Remove(Tag tag) => _context.Tags.Remove(tag);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
