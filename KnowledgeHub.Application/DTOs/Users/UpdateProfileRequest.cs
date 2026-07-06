using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeHub.Application.DTOs.Users
{
    public class UpdateProfileRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
