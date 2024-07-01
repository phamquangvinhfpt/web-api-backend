using BusinessObject.Models;
using Core.Models;
using Core.Models.AuthModel;

namespace Core.Auth.Services
{
    public interface ITokenService
    {
        Task<TokenModel> GenerateToken(AppUser user, string deviceId, bool isMobile, string ipAddress);

        Task<ResponseManager> AddRefreshToken(Token token, string deviceId, bool isMobile);

        Task<ResponseManager> UpdateRefreshToken(Token token);

        Task<Token> GetToken(string accessToken);

        Task<Token> GetRefreshToken(string token);
        Task<List<Token>> GetTokens(Guid userId);
        string GenerateRefreshToken();
        Task<ResponseManager> RevokeToken(List<Token> tokens);
        Task<bool> IsTokenRevoked(string token);
        Task CleanupTokens();
    }
}