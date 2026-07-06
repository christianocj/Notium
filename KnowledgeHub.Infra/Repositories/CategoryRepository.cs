using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KnowledgeHub.Infra.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) => _context = context;

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => await _context.Categories
                .Where(c => c.UserId == userId)
                .Include(c => c.Notes)
                .ToListAsync(ct);

        public async Task AddAsync(Category category, CancellationToken ct = default)
            => await _context.Categories.AddAsync(category, ct);

        public void Update(Category category) => _context.Categories.Update(category);
        public void Remove(Category category) => _context.Categories.Remove(category);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
