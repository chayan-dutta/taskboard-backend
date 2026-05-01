using Microsoft.AspNetCore.Mvc;
using TaskBoard.AuthService.Application;

namespace TaskBoard.AuthService.API;

[ApiController]
[Route("internal/auth")]
public class InternalAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public InternalAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // GET USER BY ID
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _authService.GetCurrentUserAsync(userId);
        return Ok(user);
    }

    // CHECK USER EXISTS
    [HttpGet("users/{userId}/exists")]
    public async Task<IActionResult> UserExists(Guid userId)
    {
        var user = await _authService.GetCurrentUserAsync(userId);

        return Ok(new
        {
            exists = user != null
        });
    }

    // GET USER ROLES
    [HttpGet("users/{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        var user = await _authService.GetCurrentUserAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            roles = user.Roles
        });
    }
}