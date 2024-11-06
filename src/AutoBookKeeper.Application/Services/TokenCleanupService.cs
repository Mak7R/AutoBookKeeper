using AutoBookKeeper.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoBookKeeper.Application.Services;

public class TokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval;

    public TokenCleanupService(IServiceProvider serviceProvider, TimeSpan interval)
    {
        _serviceProvider = serviceProvider;
        _interval = interval;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await CleanupExpiredTokens(cancellationToken);
            
            await Task.Delay(_interval, cancellationToken);
        }
    }

    private async Task CleanupExpiredTokens(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var tokenRepository = scope.ServiceProvider.GetRequiredService<IUserTokensRepository>();

        await tokenRepository.RemoveExpiredTokens();
    }
}