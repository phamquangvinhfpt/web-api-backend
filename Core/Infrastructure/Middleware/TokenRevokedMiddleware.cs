using System.Net;
using Core.Auth.Services;

namespace Core.Infrastructure.Middleware
{
    public class TokenRevokedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenRevokedMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string token = await GetTokenFromRequest(context.Request);

            if (token != null)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

                    // Kiểm tra xem token đã bị revoked chưa
                    var isTokenRevoked = await tokenService.IsTokenRevoked(token);
                    if (isTokenRevoked)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }
            }

            // Gọi middleware tiếp theo trong pipeline
            await _next(context);
        }

        private async Task<string> GetTokenFromRequest(HttpRequest request)
        {
            // Lấy token từ header Authorization
            var authHeader = request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length);
            }

            return null;
        }
    }
}