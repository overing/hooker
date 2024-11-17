using Microsoft.Extensions.Logging;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

sealed class KeyboardBehaviorSimulator(ILogger<KeyboardBehaviorSimulator> logger) : BehaviorSimulator(logger)
{
    KeyCode _keyCode;

    protected override async ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken)
    {
        KeyCode newKeyCode;
        do
            newKeyCode = KeyCode.Vc1 + (ushort)Random.Shared.Next(4);
        while (newKeyCode == _keyCode);
        _keyCode = newKeyCode;
        await simulator.SimulateKeyClickAsync(_keyCode, 40, 80, cancellationToken);
    }
}
