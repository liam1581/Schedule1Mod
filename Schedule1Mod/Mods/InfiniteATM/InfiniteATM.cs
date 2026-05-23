using Il2CppScheduleOne.Money;

namespace Schedule1Mod.Mods.InfiniteATM;

public class InfiniteATM
{
    public static void Update()
    {
        ATM.WEEKLY_DEPOSIT_LIMIT = 0f;
        ATM.DepositLimitEnabled = false;
    }
}