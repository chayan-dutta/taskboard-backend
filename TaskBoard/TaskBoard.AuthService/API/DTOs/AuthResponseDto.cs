using System.Text.Json.Serialization;

namespace TaskBoard.AuthService.API.DTOs;

public class AuthResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }
}
