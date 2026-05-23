using System.Reflection;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.UI.ATM;
using MelonLoader;

namespace Schedule1Mod.Util;

public class CashHelper
{
    private static PropertyInfo cashProperty;
    private static bool searched;
    public static float LastSelectedAmount { get; set; }
    public static bool LastWasMaxButton { get; set; }
    public static float LastButtonValue { get; set; }
    public static float AccumulatedAmount { get; set; }
    public static bool WasWithdrawalMode { get; set; }

    public static float GetPlayerCash()
    {
        try
        {
            MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
            if (instance == null)
            {
                return 0f;
            }

            if (!searched)
            {
                cashProperty = instance.GetType().GetProperty("cashBalance", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                searched = true;
            }

            return (cashProperty != null) ? (float)cashProperty.GetValue(instance) : 0f;
        }
        catch (Exception e)
        {
            MelonLogger.Error("[ATM] Error getting cash: " + e);
            return 0f;
        }
    }

    public static float GetOnlineBalance(ATMInterface instance)
    {
        try
        {
            PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            float num = 0f;
            //PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!(propertyInfo.PropertyType == typeof(float)))
                {
                    continue;
                }

                try
                {
                    float num2 = (float)propertyInfo.GetValue(instance);
                    if (num2 > num && num2 > 0f && num2 < 999000000f)
                    {
                        num = num2;
                    }
                }
                catch {}
            }
            return num;
        }
        catch (Exception e)
        {
            MelonLogger.Error("[ATM] Error getting online balance: " + e.Message);
            return 0f;
        }
    }

    public static float MapButtonValue(float originalValue)
    {
        float result = ((originalValue == 20f)
            ? 75f
            : ((originalValue == 50f)
                ? 1000f
                : ((originalValue == 100f) ? 5000f :
                    (originalValue == 500f) ? 10000f : ((originalValue != 1000f) ? originalValue : 50000f))));
        return result;
    }

    public static string GetButtonLabel(float originalValue)
    {
        string result = ((originalValue == 20f) ? "$75" : ((originalValue == 50f) ? "$1K" : ((originalValue == 100f) ? "$5K" : ((originalValue == 500f) ? "$10K" : ((originalValue != 1000f) ? $"${originalValue:N0}" : "$50K")))));
        return result;
    }
}