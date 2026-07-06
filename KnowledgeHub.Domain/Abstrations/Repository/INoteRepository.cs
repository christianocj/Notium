using KnowledgeHub.Domain.Entities;

namespace KnowledgeHub.Domain.Abstrations.Repository
{
    public interface INoteRepository
    {
        Task<Note?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<(IEnumerable<Note> Items, int TotalCount)> GetPagedAsync(
            Guid userId, int page, int pageSize,
            string? sortBy, bool descending,
            CancellationToken ct = default);

        Task<IEnumerable<Note>> SearchByTitleAsync(Guid userId, string query, CancellationToken ct = default);
        Task<IEnumerable<Note>> SearchByContentAsync(Guid userId, string query, CancellationToken ct = default);
        Task<IEnumerable<Note>> GetByCategoryAsync(Guid userId, Guid categoryId, CancellationToken ct = default);
        Task<IEnumerable<Note>> GetArchivedAsync(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<Note>> GetFavoritesAsync(Guid userId, CancellationToken ct = default);

        Task AddAsync(Note note, CancellationToken ct = default);
        void Update(Note note);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
