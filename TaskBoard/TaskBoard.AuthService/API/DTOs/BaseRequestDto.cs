using System.Text.Json.Serialization;

namespace TaskBoard.AuthService.API.DTOs;

public class BaseRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
