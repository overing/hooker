using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpHook;

namespace Hooker.Core;

public interface IBehaviorSimulator
{
    void Resume();
    void Pause();
}

abstract class BehaviorSimulator(ILogger logger) : BackgroundService, IBehaviorSimulator
{
    CancellationTokenSource _pausedTokenSource = new();

    EventSimulator _simulator = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _pausedTokenSource.Cancel();

        while (!stoppingToken.IsCancellationRequested)
        {
            while (_pausedTokenSource.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromMilliseconds(33), stoppingToken);

            await ActionAsync(_simulator, stoppingToken);

            await Task.Delay(Random.Shared.Next(40, 80), stoppingToken);
        }
    }

    public void Resume()
    {
        if (!_pausedTokenSource.IsCancellationRequested)
            return;

        logger.LogInformation(nameof(Resume));
        _pausedTokenSource.Dispose();
        _pausedTokenSource = new();
    }

    public void Pause()
    {
        if (_pausedTokenSource.IsCancellationRequested)
            return;

        logger.LogInformation(nameof(Pause));
        _pausedTokenSource.Cancel();
    }

    protected abstract ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken);
}
