using Microsoft.Extensions.Logging;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

sealed class MouseBehaviorSimulator(ILogger<MouseBehaviorSimulator> logger) : BehaviorSimulator(logger)
{
    protected override ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken)
        => simulator.SimulateMouseClickAsync(MouseButton.Button1, 33, 80, cancellationToken);
}
