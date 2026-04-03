using System.Text.Json.Serialization;

namespace TaskBoard.AuthService.API.DTOs;

public class RegisterRequestDto : BaseRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}
