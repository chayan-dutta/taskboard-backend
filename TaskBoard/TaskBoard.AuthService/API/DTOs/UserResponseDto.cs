namespace TaskBoard.AuthService.API.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Expose roles as string for API clarity
    public List<string> Roles { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
