using KnowledgeHub.Application.DTOs.Notes;
using KnowledgeHub.Application.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.Abstrations.Services
{
    public interface ITagService
    {
        Task<TagDto> CreateAsync(Guid userId, CreateTagRequest request, CancellationToken ct = default);
        Task<IEnumerable<TagDto>> GetAllAsync(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<NoteDto>> GetNotesByTagAsync(Guid userId, string tagName, CancellationToken ct = default);
        Task DeleteAsync(Guid userId, Guid tagId, CancellationToken ct = default);
    }
}
