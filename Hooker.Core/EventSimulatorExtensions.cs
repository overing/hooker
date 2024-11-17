using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

static class EventSimulatorExtensions
{
    public static async ValueTask SimulateMouseClickAsync(
        this EventSimulator simulator,
        MouseButton button,
        int minMsDelay,
        int maxMsDelay,
        CancellationToken cancellationToken = default)
    {
        simulator.SimulateMousePress(button);
        await Task.Delay(Random.Shared.Next(minMsDelay, maxMsDelay), cancellationToken);

        simulator.SimulateMouseRelease(button);
    }

    public static async ValueTask SimulateKeyClickAsync(
        this EventSimulator simulator,
        KeyCode keyCode,
        int minMsDelay,
        int maxMsDelay,
        CancellationToken cancellationToken = default)
    {
        simulator.SimulateKeyPress(keyCode);
        await Task.Delay(Random.Shared.Next(minMsDelay, maxMsDelay), cancellationToken);

        simulator.SimulateKeyRelease(keyCode);
    }
}
