using Microsoft.Extensions.Logging;
using SharpHook;
using SharpHook.Native;

namespace Hooker.Core;

sealed class MouseBehaviorSimulator(ILogger<MouseBehaviorSimulator> logger) : BehaviorSimulator(logger)
{
    readonly Random _random = new();

    const int Radius = 128;

    (short x, short y) _center;
    bool _cycleMovement;
    int _angle;

    protected override void OnResume(ResumeArguments args)
    {
        base.OnResume(args);

        logger.LogInformation("{args}", args);

        _center = (args.MouseX, args.MouseY);
        _cycleMovement = args.CycleMovement;
        _angle = 0;
    }

    protected override ValueTask ActionAsync(EventSimulator simulator, CancellationToken cancellationToken)
    {
        if (_cycleMovement)
        {
            if (_angle >= 360)
                _angle = 0;
            else
                _angle += _random.Next(15, 45);

            var radians = _angle * Math.PI / 180.0;
            var x = (short)(_center.x + Radius * Math.Cos(radians));
            var y = (short)(_center.y + Radius * Math.Sin(radians));
            simulator.SimulateMouseMovement(x, y);
        }

        return simulator.SimulateMouseClickAsync(MouseButton.Button1, 33, 80, cancellationToken);
    }
}
