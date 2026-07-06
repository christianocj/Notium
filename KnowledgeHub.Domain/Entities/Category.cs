using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
