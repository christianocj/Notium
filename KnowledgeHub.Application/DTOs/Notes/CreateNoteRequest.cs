using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.DTOs.Notes;

public class CreateNoteRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public List<string>? Tags { get; set; }
}

public class UpdateNoteRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}