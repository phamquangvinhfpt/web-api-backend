using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Auth.Services;

namespace Core.Infrastructure.Hangfire
{
    public class TokenCleanupJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TokenCleanupJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    // Method invoked by Hangfire
    public async Task CleanupTokens()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            
            await tokenService.CleanupTokens();
        }
    }
}
}