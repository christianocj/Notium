using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
