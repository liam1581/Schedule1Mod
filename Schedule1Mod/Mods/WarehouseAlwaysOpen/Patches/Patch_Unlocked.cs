using HarmonyLib;
using Il2CppScheduleOne.Map;

namespace Schedule1Mod.Mods.WarehouseAlwaysOpen.Patches
{
    [HarmonyPatch(typeof(DarkMarket), "get_Unlocked")]
    public static class Patch_Unlocked
    {
        private static void Postfix(ref bool __result)
        {
            if (!__result && (bool)Preferences.Settings["AlwaysOpen"])
            {
                __result = true;
            }
        }
    }
}
