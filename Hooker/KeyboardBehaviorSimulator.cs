using Microsoft.Extensions.Logging;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

sealed class KeyboardBehaviorSimulator(ILogger<KeyboardBehaviorSimulator> logger) : BehaviorSimulator(logger)
{
    static readonly IReadOnlyList<KeyCode> AllowKeys =
    [
        KeyCode.Vc3,
        KeyCode.Vc4,
        KeyCode.Vc5,
        KeyCode.Vc6,
        KeyCode.Vc7,
        KeyCode.Vc8,
        KeyCode.Vc9,
        KeyCode.Vc0,
    ];

    protected override async ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken)
    {
        var newKeyCode = AllowKeys[Random.Shared.Next(0, AllowKeys.Count)];
        await simulator.SimulateKeyClickAsync(newKeyCode, 11, 40, cancellationToken);
    }
}
