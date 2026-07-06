using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.DTOs.Notes;
using KnowledgeHub.Application.Mappers;
using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public NoteService(INoteRepository noteRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task<NoteDto> CreateAsync(Guid userId, CreateNoteRequest request, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
            if (category is null || category.UserId != userId)
                throw new KeyNotFoundException("Categoria não encontrada.");

            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                CategoryId = request.CategoryId,
                UserId = userId
            };

            if (request.Tags is { Count: > 0 })
                await AttachTagsAsync(note, userId, request.Tags, ct);

            await _noteRepository.AddAsync(note, ct);
            await _noteRepository.SaveChangesAsync(ct);

            note.Category = category;
            return NoteMapper.MapToDto(note);
        }

        public async Task<NoteDto> GetByIdAsync(Guid userId, Guid noteId, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);
            return NoteMapper.MapToDto(note);
        }

        public async Task<PagedResult<NoteDto>> GetAllAsync(Guid userId, int page, int pageSize, string? sortBy, bool descending, CancellationToken ct = default)
        {
            var (items, totalCount) = await _noteRepository.GetPagedAsync(userId, page, pageSize, sortBy, descending, ct);

            return new PagedResult<NoteDto>
            {
                Items = items.Select(NoteMapper.MapToDto).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<NoteDto> UpdateAsync(Guid userId, Guid noteId, UpdateNoteRequest request, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
            if (category is null || category.UserId != userId)
                throw new KeyNotFoundException("Categoria não encontrada.");

            note.Title = request.Title;
            note.Content = request.Content;
            note.CategoryId = request.CategoryId;
            note.UpdatedAt = DateTime.UtcNow;

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync(ct);

            note.Category = category;
            return NoteMapper.MapToDto(note);
        }

        public async Task DeleteAsync(Guid userId, Guid noteId, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);

            note.IsDeleted = true;
            note.DeletedAt = DateTime.UtcNow;

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync(ct);
        }

        public async Task<NoteDto> ToggleArchiveAsync(Guid userId, Guid noteId, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);

            note.IsArchived = !note.IsArchived;
            note.ArchivedAt = note.IsArchived ? DateTime.UtcNow : null;
            note.UpdatedAt = DateTime.UtcNow;

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync(ct);

            return NoteMapper.MapToDto(note);
        }

        public async Task<IEnumerable<NoteDto>> GetArchivedAsync(Guid userId, CancellationToken ct = default)
        {
            var notes = await _noteRepository.GetArchivedAsync(userId, ct);
            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task<NoteDto> ToggleFavoriteAsync(Guid userId, Guid noteId, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);

            note.IsFavorite = !note.IsFavorite;
            note.UpdatedAt = DateTime.UtcNow;

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync(ct);

            return NoteMapper.MapToDto(note);
        }

        public async Task<IEnumerable<NoteDto>> GetFavoritesAsync(Guid userId, CancellationToken ct = default)
        {
            var notes = await _noteRepository.GetFavoritesAsync(userId, ct);
            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task<IEnumerable<NoteDto>> SearchByTitleAsync(Guid userId, string query, CancellationToken ct = default)
        {
            var notes = await _noteRepository.SearchByTitleAsync(userId, query, ct);
            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task<IEnumerable<NoteDto>> SearchByContentAsync(Guid userId, string query, CancellationToken ct = default)
        {
            var notes = await _noteRepository.SearchByContentAsync(userId, query, ct);
            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task<IEnumerable<NoteDto>> GetByCategoryAsync(Guid userId, Guid categoryId, CancellationToken ct = default)
        {
            var notes = await _noteRepository.GetByCategoryAsync(userId, categoryId, ct);
            return notes.Select(NoteMapper.MapToDto);
        }

        public async Task AddTagAsync(Guid userId, Guid noteId, string tagName, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);
            await AttachTagsAsync(note, userId, new List<string> { tagName }, ct);

            _noteRepository.Update(note);
            await _noteRepository.SaveChangesAsync(ct);
        }

        public async Task RemoveTagAsync(Guid userId, Guid noteId, Guid tagId, CancellationToken ct = default)
        {
            var note = await GetOwnedNoteAsync(userId, noteId, ct);
            var tag = note.Tags.FirstOrDefault(t => t.Id == tagId);

            if (tag is not null)
            {
                note.Tags.Remove(tag);
                _noteRepository.Update(note);
                await _noteRepository.SaveChangesAsync(ct);
            }
        }

        private async Task<Note> GetOwnedNoteAsync(Guid userId, Guid noteId, CancellationToken ct)
        {
            var note = await _noteRepository.GetByIdAsync(noteId, ct)
                ?? throw new KeyNotFoundException("Nota não encontrada.");

            if (note.UserId != userId)
                throw new UnauthorizedAccessException("Você não tem acesso a esta nota.");

            return note;
        }

        private async Task AttachTagsAsync(Note note, Guid userId, List<string> tagNames, CancellationToken ct)
        {
            foreach (var name in tagNames.Distinct())
            {
                var tag = await _tagRepository.GetByNameAsync(userId, name, ct);

                if (tag is null)
                {
                    tag = new Tag { Name = name, UserId = userId };
                    await _tagRepository.AddAsync(tag, ct);
                }

                if (!note.Tags.Any(t => t.Name == name))
                    note.Tags.Add(tag);
            }
        }
    }
}
