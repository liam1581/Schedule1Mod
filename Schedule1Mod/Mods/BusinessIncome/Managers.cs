using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.UI;

namespace Schedule1Mod.Mods.BusinessIncome;

internal sealed class Managers
{
    public static bool IsInitialized => Notifications != null && Time != null && Money != null;
    public static NotificationsManager Notifications { get; private set; }
    public static TimeManager Time { get; private set; }
    public static MoneyManager Money { get; private set; }

    public static bool Get()
    {
        Notifications = Helper.GetSingleton<NotificationsManager>();
        Time = Helper.GetNetSingleton<TimeManager>();
        Money = Helper.GetNetSingleton<MoneyManager>();

        return IsInitialized;
    }
}