using HarmonyLib;
using Il2CppScheduleOne.Quests;
using MelonLoader;

namespace Schedule1Mod.Patches
{
    [HarmonyPatch(typeof(Quest_WelcomeToHylandPoint), "Explode")]
    public class Patch_Quest_WelcomeToHylandPoint
    {
        [HarmonyPrefix]
        public static bool Prefix(Quest_WelcomeToHylandPoint __instance)
        {
            MelonLogger.Msg("Patched Quest_WelcomeToHylandPoint");
            return false;
        }
    }
}
