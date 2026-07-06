using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.DTOs.Users;
using KnowledgeHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KnowledgeHub.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    [EnableRateLimiting("fixed")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(CancellationToken ct)
            => Ok(await _userService.GetProfileAsync(CurrentUserId, ct));

        [HttpPut("profile")]
        public async Task<ActionResult<UserProfileDto>> UpdateProfile(UpdateProfileRequest request, CancellationToken ct)
            => Ok(await _userService.UpdateProfileAsync(CurrentUserId, request, ct));
    }
}
