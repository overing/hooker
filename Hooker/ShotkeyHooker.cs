using Microsoft.Extensions.Hosting;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

public sealed class ShotkeyHooker(
    IEnumerable<IBehaviorSimulator> behaviors) : BackgroundService
{
    readonly IReadOnlyCollection<IBehaviorSimulator> _behaviors = [.. behaviors];

    readonly ResumeArguments _args = new() { CycleMovement = true };

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var hook = new TaskPoolGlobalHook();
        hook.MouseReleased += OnMouseReleased;
        hook.MouseMoved += OnMouseMoved;
        return hook.RunAsync();
    }

    void ResumeBehaviors()
    {
        foreach (var behavior in _behaviors)
        {
            if (_args.KeyboardOnly && behavior is not KeyboardBehaviorSimulator)
                continue;
            behavior.Resume(_args);
        }
    }

    void PauseBehaviors()
    {
        foreach (var behavior in _behaviors)
            behavior.Pause();
    }

    void OnMouseReleased(object? sender, MouseHookEventArgs eventArgs)
    {
        if (eventArgs.IsEventSimulated)
            return;

        switch (eventArgs.RawEvent.Mouse.Button)
        {
            case MouseButton.Button5: // 滑鼠翻頁上
                _args.CycleMovement = true;
                ResumeBehaviors();
                break;

            case MouseButton.Button4: // 滑鼠翻頁下
                _args.CycleMovement = false;
                ResumeBehaviors();
                break;

            case MouseButton.Button3: // 滑鼠滾輪按下
                _args.KeyboardOnly = !_args.KeyboardOnly;
                break;
        }
    }

    void OnMouseMoved(object? sender, MouseHookEventArgs eventArgs)
    {
        if (eventArgs.IsEventSimulated)
            return;

        var data = eventArgs.Data;
        (_args.MouseX, _args.MouseY) = (data.X, data.Y);

        PauseBehaviors();
    }
}
