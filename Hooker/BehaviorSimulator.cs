using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpHook;

namespace Hooker.Core;

public interface IBehaviorSimulator
{
    void Resume(ResumeArguments args);
    void Pause();
}

abstract class BehaviorSimulator(ILogger logger) : BackgroundService, IBehaviorSimulator
{
    CancellationTokenSource _pausedTokenSource = new();

    readonly EventSimulator _simulator = new();

    readonly Random _random = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _pausedTokenSource.Cancel();

        while (!stoppingToken.IsCancellationRequested)
        {
            while (_pausedTokenSource.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromMilliseconds(33), stoppingToken);

            await ActionAsync(_simulator, stoppingToken);

            await Task.Delay(_random.Next(40, 80), stoppingToken);
        }
    }

    public void Resume(ResumeArguments args)
    {
        if (!_pausedTokenSource.IsCancellationRequested)
            return;

        logger.LogInformation(nameof(Resume));

        _pausedTokenSource.Dispose();
        _pausedTokenSource = new();

        OnResume(args);
    }

    protected virtual void OnResume(ResumeArguments args) { }

    public void Pause()
    {
        if (_pausedTokenSource.IsCancellationRequested)
            return;

        _pausedTokenSource.Cancel();

        OnPause();

        logger.LogInformation(nameof(Pause));
    }

    protected virtual void OnPause() { }

    protected abstract ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken);
}
