using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Notes;
using KnowledgeHub.Application.DTOs.Tags;
using KnowledgeHub.Application.Mappers;
using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly INoteRepository _noteRepository;

        public TagService(ITagRepository tagRepository, INoteRepository noteRepository)
        {
            _tagRepository = tagRepository;
            _noteRepository = noteRepository;
        }

        public async Task<TagDto> CreateAsync(Guid userId, CreateTagRequest request, CancellationToken ct = default)
        {
            var existing = await _tagRepository.GetByNameAsync(userId, request.Name, ct);
            if (existing is not null)
                throw new InvalidOperationException("Já existe uma tag com este nome.");

            var tag = new Tag { Name = request.Name, UserId = userId };

            await _tagRepository.AddAsync(tag, ct);
            await _tagRepository.SaveChangesAsync(ct);

            return MapToDto(tag);
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync(Guid userId, CancellationToken ct = default)
        {
            var tags = await _tagRepository.GetByUserIdAsync(userId, ct);
            return tags.Select(MapToDto);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByTagAsync(Guid userId, string tagName, CancellationToken ct = default)
        {
            var tag = await _tagRepository.GetByNameAsync(userId, tagName, ct)
                ?? throw new KeyNotFoundException("Tag não encontrada.");

            // Busca completa: aqui reaproveitamos GetByIdAsync do Note para trazer com includes,
            // mas o ideal é ter um método dedicado no INoteRepository se a lista crescer muito.
            var allUserNotes = await _noteRepository.GetPagedAsync(userId, 1, int.MaxValue, null, true, ct);
            var notes = allUserNotes.Items.Where(n => n.Tags.Any(t => t.Id == tag.Id));

            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task DeleteAsync(Guid userId, Guid tagId, CancellationToken ct = default)
        {
            var tag = await _tagRepository.GetByIdAsync(tagId, ct)
                ?? throw new KeyNotFoundException("Tag não encontrada.");

            if (tag.UserId != userId)
                throw new UnauthorizedAccessException("Você não tem acesso a esta tag.");

            _tagRepository.Remove(tag);
            await _tagRepository.SaveChangesAsync(ct);
        }

        private static TagDto MapToDto(Tag tag) => new()
        {
            Id = tag.Id,
            Name = tag.Name,
            CreatedAt = tag.CreatedAt
        };
    }
}
