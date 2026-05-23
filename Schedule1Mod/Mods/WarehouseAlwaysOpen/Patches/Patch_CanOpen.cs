using Il2CppScheduleOne.Doors;
using HarmonyLib;

namespace Schedule1Mod.Mods.WarehouseAlwaysOpen.Patches
{
    [HarmonyPatch(typeof(DarkMarketRollerDoors), "CanOpen")]
    public static class Patch_CanOpen
    {
        [HarmonyPostfix]
        private static void Postfix(ref bool __result)
        {
            if (!__result && (bool)Preferences.Settings["AlwaysOpen"])
            {
                __result = true; 
            }
        }        
    }
}
