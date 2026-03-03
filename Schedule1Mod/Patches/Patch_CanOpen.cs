using Il2CppScheduleOne.Doors;
using HarmonyLib;
using Schedule1Mod.Config;

namespace Schedule1Mod.Patches
{
    [HarmonyPatch(typeof(DarkMarketRollerDoors), "CanOpen")]
    public static class Patch_CanOpen
    {
        [HarmonyPostfix]
        private static void Postfix(ref bool __result)
        {
            if (!__result && ConfigManager.Config.AlwaysOpen)
            {
                __result = true; 
            }
        }        
    }
}
