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
        hook.KeyReleased += OnKeyReleased;
        return hook.RunAsync();
    }

    void ResumeBehaviors()
    {
        foreach (var behavior in _behaviors)
            behavior.Resume();
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
                ResumeBehaviors();
                break;

            case MouseButton.Button4:
                PauseBehaviors();
                break;
        }
    }

    void OnMouseMoved(object? sender, MouseHookEventArgs eventArgs) => PauseBehaviors();

    void OnKeyReleased(object? sender, KeyboardHookEventArgs eventArgs)
    {
        switch (eventArgs.RawEvent.Keyboard.KeyCode)
        {
            case KeyCode.VcF2:
                ResumeBehaviors();
                break;

            case KeyCode.VcF3:
                PauseBehaviors();
                break;
        }
    }
}
