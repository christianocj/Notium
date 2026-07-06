using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KnowledgeHub.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableRateLimiting("fixed")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResult>> Register(RegisterRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _authService.RegisterAsync(request, ct);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _authService.LoginAsync(request, ct);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
