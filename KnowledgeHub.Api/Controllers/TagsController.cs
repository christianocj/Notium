using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Notes;
using KnowledgeHub.Application.DTOs.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KnowledgeHub.Api.Controllers
{
    [ApiController]
    [Route("api/tags")]
    [Authorize]
    [EnableRateLimiting("fixed")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService) => _tagService = tagService;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpPost]
        public async Task<ActionResult<TagDto>> Create(CreateTagRequest request, CancellationToken ct)
            => Ok(await _tagService.CreateAsync(CurrentUserId, request, ct));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll(CancellationToken ct)
            => Ok(await _tagService.GetAllAsync(CurrentUserId, ct));

        [HttpGet("{name}/notes")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesByTag(string name, CancellationToken ct)
            => Ok(await _tagService.GetNotesByTagAsync(CurrentUserId, name, ct));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _tagService.DeleteAsync(CurrentUserId, id, ct);
            return NoContent();
        }
    }
}
