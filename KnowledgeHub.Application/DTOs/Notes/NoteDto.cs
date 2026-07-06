using KnowledgeHub.Application.DTOs.Tags;

namespace KnowledgeHub.Application.DTOs.Notes
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public bool IsFavorite { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<TagDto> Tags { get; set; } = new();
    }
}
