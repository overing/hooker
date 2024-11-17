using Microsoft.Extensions.DependencyInjection;

namespace Hooker.Core;

public static class ServiceCollectionHookerCoreExtensions
{
    public static IServiceCollection AddHookerCore(this IServiceCollection collection)
    {
        collection.AddSingleton<MouseBehaviorSimulator>();
        collection.AddSingleton<KeyboardBehaviorSimulator>();

        collection.AddSingleton<IBehaviorSimulator>(p => p.GetRequiredService<MouseBehaviorSimulator>());
        collection.AddSingleton<IBehaviorSimulator>(p => p.GetRequiredService<KeyboardBehaviorSimulator>());

        collection.AddHostedService<ShotkeyHooker>();
        collection.AddHostedService(p => p.GetRequiredService<MouseBehaviorSimulator>());
        collection.AddHostedService(p => p.GetRequiredService<KeyboardBehaviorSimulator>());

        return collection;
    }
}
