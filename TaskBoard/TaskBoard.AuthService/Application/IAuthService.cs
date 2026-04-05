using TaskBoard.AuthService.API.DTOs;

namespace TaskBoard.AuthService.Application;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    Task<UserResponseDto> GetCurrentUserAsync(Guid userId);
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto);
}
