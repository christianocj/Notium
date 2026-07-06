using KnowledgeHub.Application.DTOs.Notes;
using KnowledgeHub.Application.DTOs.Tags;
using KnowledgeHub.Domain.Entities;
using System;

namespace KnowledgeHub.Application.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto MapToDto(Note note) => new()
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            IsArchived = note.IsArchived,
            ArchivedAt = note.ArchivedAt,
            IsFavorite = note.IsFavorite,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
            CategoryId = note.CategoryId,
            CategoryName = note.Category?.Name ?? string.Empty,
            Tags = note.Tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                CreatedAt = t.CreatedAt
            }).ToList()
        };
    }
}
