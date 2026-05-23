using Il2CppScheduleOne.DevUtilities;
using MelonLoader;

namespace Schedule1Mod.Mods.BusinessIncome;

public class Helper
{
    public static T GetSingleton<T>() where T : Singleton<T>
    {
        var instance = Singleton<T>.Instance;
        LogFound(instance);
        return instance;
    }

    public static T GetNetSingleton<T>() where T : NetworkSingleton<T>
    {
        var instance = NetworkSingleton<T>.Instance;
        LogFound(instance);
        return instance;
    }

    private static void LogFound<T>(T obj)
    {
        if (obj == null)
            MelonLogger.Warning($"Failed to find {typeof(T).Name}");
        else
            MelonLogger.Msg($"Found {typeof(T).Name}");
    }
}