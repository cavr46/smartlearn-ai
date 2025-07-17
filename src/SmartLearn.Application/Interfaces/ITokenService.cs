using SmartLearn.Domain.Entities;

namespace SmartLearn.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
}