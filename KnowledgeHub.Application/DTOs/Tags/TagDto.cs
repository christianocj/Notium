using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.DTOs.Tags
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTagRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
