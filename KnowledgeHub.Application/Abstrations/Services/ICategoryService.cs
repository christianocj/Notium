using KnowledgeHub.Application.DTOs.Categories;
using System;

namespace KnowledgeHub.Application.Abstrations.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request, CancellationToken ct = default);
        Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId, CancellationToken ct = default);
        Task<CategoryDto> UpdateAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid userId, Guid categoryId, CancellationToken ct = default);
    }
}
