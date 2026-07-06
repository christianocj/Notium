using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Categories;
using KnowledgeHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KnowledgeHub.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    [EnableRateLimiting("fixed")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryRequest request, CancellationToken ct)
        {
            var category = await _categoryService.CreateAsync(CurrentUserId, request, ct);
            return CreatedAtAction(nameof(GetAll), category);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken ct)
            => Ok(await _categoryService.GetAllAsync(CurrentUserId, ct));

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(Guid id, UpdateCategoryRequest request, CancellationToken ct)
            => Ok(await _categoryService.UpdateAsync(CurrentUserId, id, request, ct));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _categoryService.DeleteAsync(CurrentUserId, id, ct);
            return NoContent();
        }
    }
}
