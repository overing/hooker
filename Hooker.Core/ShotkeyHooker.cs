using Microsoft.Extensions.Hosting;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

public sealed class ShotkeyHooker(
    IEnumerable<IBehaviorSimulator> behaviors) : BackgroundService
{
    IReadOnlyCollection<IBehaviorSimulator> _behaviors = behaviors.ToList();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var hook = new TaskPoolGlobalHook();
        hook.MouseReleased += OnMouseReleased;
        hook.MouseMoved += OnMouseMoved;
        return hook.RunAsync();
    }

    void ResumeBehaviors(bool keyboardOnly)
    {
        foreach (var behavior in _behaviors)
        {
            if (keyboardOnly && behavior is not KeyboardBehaviorSimulator)
                continue;
            behavior.Resume();
        }
    }

    void PauseBehaviors()
    {
        foreach (var behavior in _behaviors)
            behavior.Pause();
    }

    void OnMouseReleased(object? sender, MouseHookEventArgs eventArgs)
    {
        switch (eventArgs.RawEvent.Mouse.Button)
        {
            case MouseButton.Button5:
                ResumeBehaviors(keyboardOnly: false);
                break;

            case MouseButton.Button4:
                ResumeBehaviors(keyboardOnly: true);
                break;
        }
    }

    void OnMouseMoved(object? sender, MouseHookEventArgs eventArgs) => PauseBehaviors();
}
