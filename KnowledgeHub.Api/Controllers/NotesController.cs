using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs;
using KnowledgeHub.Application.DTOs.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KnowledgeHub.Api.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    [EnableRateLimiting("fixed")]
   
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService) => _noteService = noteService;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        
        [HttpPost]
        public async Task<ActionResult<NoteDto>> Create(CreateNoteRequest request, CancellationToken ct)
        {
            var note = await _noteService.CreateAsync(CurrentUserId, request, ct);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetById(Guid id, CancellationToken ct)
            => Ok(await _noteService.GetByIdAsync(CurrentUserId, id, ct));

        [EnableRateLimiting("notes-policy")]
        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<NoteDto>>> GetAll(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null, [FromQuery] bool descending = true,
            CancellationToken ct = default)
            => Ok(await _noteService.GetAllAsync(CurrentUserId, page, pageSize, sortBy, descending, ct));

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> Update(Guid id, UpdateNoteRequest request, CancellationToken ct)
            => Ok(await _noteService.UpdateAsync(CurrentUserId, id, request, ct));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _noteService.DeleteAsync(CurrentUserId, id, ct);
            return NoContent();
        }

        [HttpPut("{id}/archive")]
        public async Task<ActionResult<NoteDto>> ToggleArchive(Guid id, CancellationToken ct)
            => Ok(await _noteService.ToggleArchiveAsync(CurrentUserId, id, ct));

        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetArchived(CancellationToken ct)
            => Ok(await _noteService.GetArchivedAsync(CurrentUserId, ct));

        [HttpPut("{id}/favorite")]
        public async Task<ActionResult<NoteDto>> ToggleFavorite(Guid id, CancellationToken ct)
            => Ok(await _noteService.ToggleFavoriteAsync(CurrentUserId, id, ct));

        [HttpGet("favorites")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetFavorites(CancellationToken ct)
            => Ok(await _noteService.GetFavoritesAsync(CurrentUserId, ct));

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> Search([FromQuery] string q, [FromQuery] string by = "title", CancellationToken ct = default)
            => Ok(by == "content"
                ? await _noteService.SearchByContentAsync(CurrentUserId, q, ct)
                : await _noteService.SearchByTitleAsync(CurrentUserId, q, ct));

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetByCategory(Guid categoryId, CancellationToken ct)
            => Ok(await _noteService.GetByCategoryAsync(CurrentUserId, categoryId, ct));
    }
}
