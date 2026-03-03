using HarmonyLib;
using Il2CppScheduleOne.Map;
using Schedule1Mod.Config;

namespace Schedule1Mod.Patches
{
    
    [HarmonyPatch(typeof(DarkMarketAccessZone), "GetIsOpen")]
    public static class Patch_GetsOpen
    {
        [HarmonyPostfix]
        private static void Postfix(ref bool __result)
        {
            if (__result)
            {
                return;
            }
            if (ConfigManager.Config.RequireRank)
            {
                __result = true;
            }
            else if (ConfigManager.Config.AlwaysOpen)
            {
                __result = true;
            }
        }
    }
}
