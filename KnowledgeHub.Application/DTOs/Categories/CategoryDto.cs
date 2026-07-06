using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int NotesCount { get; set; }
    }
}
