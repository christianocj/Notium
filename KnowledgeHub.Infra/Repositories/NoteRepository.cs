using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using KnowledgeHub.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Infra.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;
        public NoteRepository(AppDbContext context) => _context = context;

        public async Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Notes
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.Id == id, ct);

        public async Task<(IEnumerable<Note> Items, int TotalCount)> GetPagedAsync(
            Guid userId, int page, int pageSize,
            string? sortBy, bool descending,
            CancellationToken ct = default)
        {
            var query = _context.Notes
                .Where(n => n.UserId == userId && !n.IsArchived)
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .AsQueryable();

            query = sortBy?.ToLower() switch
            {
                "title" => descending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
                "createdat" => descending ? query.OrderByDescending(n => n.CreatedAt) : query.OrderBy(n => n.CreatedAt),
                _ => descending ? query.OrderByDescending(n => n.UpdatedAt ?? n.CreatedAt)
                                 : query.OrderBy(n => n.UpdatedAt ?? n.CreatedAt)
            };

            var totalCount = await query.CountAsync(ct);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<IEnumerable<Note>> SearchByTitleAsync(Guid userId, string query, CancellationToken ct = default)
            => await _context.Notes
                .Where(n => n.UserId == userId && n.Title.Contains(query))
                .Include(n => n.Category).Include(n => n.Tags)
                .ToListAsync(ct);

        public async Task<IEnumerable<Note>> SearchByContentAsync(Guid userId, string query, CancellationToken ct = default)
            => await _context.Notes
                .Where(n => n.UserId == userId && n.Content.Contains(query))
                .Include(n => n.Category).Include(n => n.Tags)
                .ToListAsync(ct);

        public async Task<IEnumerable<Note>> GetByCategoryAsync(Guid userId, Guid categoryId, CancellationToken ct = default)
            => await _context.Notes
                .Where(n => n.UserId == userId && n.CategoryId == categoryId)
                .Include(n => n.Category).Include(n => n.Tags)
                .ToListAsync(ct);

        public async Task<IEnumerable<Note>> GetArchivedAsync(Guid userId, CancellationToken ct = default)
            => await _context.Notes
                .Where(n => n.UserId == userId && n.IsArchived)
                .Include(n => n.Category).Include(n => n.Tags)
                .ToListAsync(ct);

        public async Task<IEnumerable<Note>> GetFavoritesAsync(Guid userId, CancellationToken ct = default)
            => await _context.Notes
                .Where(n => n.UserId == userId && n.IsFavorite)
                .Include(n => n.Category).Include(n => n.Tags)
                .ToListAsync(ct);

        public async Task AddAsync(Note note, CancellationToken ct = default)
            => await _context.Notes.AddAsync(note, ct);

        public void Update(Note note) => _context.Notes.Update(note);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);
    }
}
