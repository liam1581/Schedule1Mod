using HarmonyLib;
using Il2CppScheduleOne.Map;

namespace Schedule1Mod.Mods.WarehouseAlwaysOpen.Patches
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
            if ((bool)Preferences.All["RequireRank"])
            {
                __result = true;
            }
            else if ((bool)Preferences.All["AlwaysOpen"])
            {
                __result = true;
            }
        }
    }
}
