using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BussinessObject.Data;
using BussinessObject.Models;
using Core.Auth.Services;
using Core.Models;
using Core.Models.AuthModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Core.Auth.Repository
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<TokenService> _logger;

        public TokenService(AppDbContext context,
                            IConfiguration config,
                            UserManager<AppUser> userManager,
                            ILogger<TokenService> logger)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }

        // GenerateToken
        // Token Genereator
        public async Task<TokenModel> GenerateToken(AppUser user, string deviceId, bool isMobile)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();

            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var roleClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FullName),
            }.Union(userClaims).Union(roleClaims).Union(userRoles);
            var tokenClaims = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenClaims);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new Token
            {
                UserId = user.Id,
                JwtId = tokenClaims.Id,
                User = user,
                AccessToken = accessToken,
                RefreshTokenHash = refreshToken,
                DeviceId = deviceId,
                IsMobile = isMobile,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(1),
            };
            await AddRefreshToken(refreshTokenEntity, deviceId, isMobile);
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // AddRefreshToken
        public async Task<ResponseManager> AddRefreshToken(Token token, string deviceId, bool isMobile)
        {
            if (token == null)
            {
                throw new NullReferenceException("Token is NULL");
            }

            List<Token> tokens = await GetTokens(token.UserId);

            // Check number of tokens
            int mobileTokenCount = tokens.Count(t => t.IsMobile == true);
            int webTokenCount = tokens.Count(t => t.IsMobile == false);

            // if token is mobile and mobile token count is greater than 0
            if (isMobile && mobileTokenCount > 0)
            {
                // Delete old mobile token
                List<Token> oldMobileTokens = tokens.Where(t => t.IsMobile == true).ToList();
                await RevokeToken(oldMobileTokens);
            }
            else if (!isMobile && webTokenCount > 0)
            {
                // Delete old web token
                List<Token> oldWebTokens = tokens.Where(t => t.IsMobile == false).ToList();
                await RevokeToken(oldWebTokens);
            }

            // Add new token
            token.DeviceId = deviceId;
            token.IsMobile = isMobile;
            token.IsUsed = false;
            token.IsRevoked = false;
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();

            return new ResponseManager
            {
                IsSuccess = true,
                Message = token,
            };
        }

        // GetRefreshToken
        public async Task<Token> GetRefreshToken(string token)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.RefreshTokenHash == token);
        }

        // GetTokens
        public async Task<List<Token>> GetTokens(Guid userId)
        {
            return await _context.Tokens.Where(x => x.UserId == userId).ToListAsync();
        }

        // UpdateRefreshToken
        public async Task<ResponseManager> UpdateRefreshToken(Token token)
        {
            _context.Tokens.Update(token);
            await _context.SaveChangesAsync();

            return new ResponseManager
            {
                IsSuccess = true,
                Message = token,
            };
        }

        // RevokeToken
        public async Task<ResponseManager> RevokeToken(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                _context.Tokens.Update(token);
            }

            await _context.SaveChangesAsync();

            return new ResponseManager
            {
                IsSuccess = true,
                Message = "Token revoked",
            };
        }

        // IsTokenRevoked
        public async Task<bool> IsTokenRevoked(string accessToken)
        {
            var tokenEntity = _context.Tokens.FirstOrDefault(t => t.AccessToken == accessToken);
            if (tokenEntity == null)
            {
                return true;
            }
            return tokenEntity.IsRevoked;
        }

        public async Task CleanupTokens()
        {
            var tokensToRemove = _context.Tokens.Where(t => t.IsUsed && t.IsRevoked);
            _context.Tokens.RemoveRange(tokensToRemove);
            await _context.SaveChangesAsync();
        }
    }
}