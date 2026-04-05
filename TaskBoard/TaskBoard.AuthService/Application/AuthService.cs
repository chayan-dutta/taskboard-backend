using TaskBoard.AuthService.API.DTOs;
using TaskBoard.AuthService.Infrastructure.Entities;
using TaskBoard.AuthService.Infrastructure.Repositories;

namespace TaskBoard.AuthService.Application;

public class AuthService(IUserRepository userRepository, IJwtService jwtService) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;

    // REGISTER
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Roles = [Role.User], // default role
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1)
        };
    }

    // LOGIN
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email) 
            ?? throw new Exception("Invalid credentials");
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Invalid credentials");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1)
        };
    }

    // GET CURRENT USER
    public async Task<UserResponseDto> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        return user == null
            ? throw new Exception("User not found")
            : new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.ToString()).ToList()
            };
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId) 
            ?? throw new Exception("User not found");

        // Verify current password
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

        if (!isPasswordValid)
            throw new Exception("Current password is incorrect");

        // Optional: prevent same password reuse
        var isSamePassword = BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.PasswordHash); 
        
        if (isSamePassword)
            throw new Exception("New password must be different from current password");

        // Hash new password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        // (optional) update timestamp if you add column later
        // user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.SaveChangesAsync();
    }
}
