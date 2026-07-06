using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.DTOs.Notes;
using System;

namespace KnowledgeHub.Application.Abstrations.Services
{
    public interface INoteService
    {
        Task<NoteDto> CreateAsync(Guid userId, CreateNoteRequest request, CancellationToken ct = default);
        Task<NoteDto> GetByIdAsync(Guid userId, Guid noteId, CancellationToken ct = default);
        Task<PagedResult<NoteDto>> GetAllAsync(Guid userId, int page, int pageSize, string? sortBy, bool descending, CancellationToken ct = default);
        Task<NoteDto> UpdateAsync(Guid userId, Guid noteId, UpdateNoteRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid userId, Guid noteId, CancellationToken ct = default);

        Task<NoteDto> ToggleArchiveAsync(Guid userId, Guid noteId, CancellationToken ct = default);
        Task<IEnumerable<NoteDto>> GetArchivedAsync(Guid userId, CancellationToken ct = default);

        Task<NoteDto> ToggleFavoriteAsync(Guid userId, Guid noteId, CancellationToken ct = default);
        Task<IEnumerable<NoteDto>> GetFavoritesAsync(Guid userId, CancellationToken ct = default);

        Task<IEnumerable<NoteDto>> SearchByTitleAsync(Guid userId, string query, CancellationToken ct = default);
        Task<IEnumerable<NoteDto>> SearchByContentAsync(Guid userId, string query, CancellationToken ct = default);
        Task<IEnumerable<NoteDto>> GetByCategoryAsync(Guid userId, Guid categoryId, CancellationToken ct = default);

        Task AddTagAsync(Guid userId, Guid noteId, string tagName, CancellationToken ct = default);
        Task RemoveTagAsync(Guid userId, Guid noteId, Guid tagId, CancellationToken ct = default);
    }
}
