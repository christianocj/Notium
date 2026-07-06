using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Categories;
using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Domain.Entities;
using System;

namespace KnowledgeHub.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

        public async Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request, CancellationToken ct = default)
        {
            var category = new Category
            {
                Name = request.Name,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category, ct);
            await _categoryRepository.SaveChangesAsync(ct);

            return MapToDto(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId, CancellationToken ct = default)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(userId, ct);
            return categories.Select(MapToDto);
        }

        public async Task<CategoryDto> UpdateAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, ct)
                ?? throw new KeyNotFoundException("Categoria não encontrada.");

            if (category.UserId != userId)
                throw new UnauthorizedAccessException("Você não tem acesso a esta categoria.");

            category.Name = request.Name;
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync(ct);

            return MapToDto(category);
        }

        public async Task DeleteAsync(Guid userId, Guid categoryId, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, ct)
                ?? throw new KeyNotFoundException("Categoria não encontrada.");

            if (category.UserId != userId)
                throw new UnauthorizedAccessException("Você não tem acesso a esta categoria.");

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync(ct);
        }

        private static CategoryDto MapToDto(Category category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            NotesCount = category.Notes?.Count ?? 0
        };
    }
}
