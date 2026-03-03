using HarmonyLib;
using Il2CppScheduleOne.Map;
using Schedule1Mod.Config;

namespace Schedule1Mod.Patches
{
    [HarmonyPatch(typeof(DarkMarket), "get_Unlocked")]
    public static class Patch_Unlocked
    {
        private static void Postfix(ref bool __result)
        {
            if (!__result && ConfigManager.Config.AlwaysOpen)
            {
                __result = true;
            }
        }
    }
}
