using TaskBoard.AuthService.Infrastructure.Entities;

namespace TaskBoard.AuthService.Application;

public interface IJwtService
{
    string GenerateToken(User user);
}
