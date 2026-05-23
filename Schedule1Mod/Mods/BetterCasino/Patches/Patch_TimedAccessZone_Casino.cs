using HarmonyLib;
using Il2CppScheduleOne.Map;
using Il2CppTMPro;
using UnityEngine;

namespace Schedule1Mod.Mods.BetterCasino.Patches
{
    [HarmonyPatch(typeof(TimedAccessZone), "Start")]
    public static class Patch_TimedAccessZone_Casino
    {
        [HarmonyPostfix]
        public static void Postfix(TimedAccessZone __instance)
        {
            var flag = __instance.gameObject.name.Equals("Casino");
            if (flag)
            {
                __instance.OpenTime = 500;
            }
            const string text = "Map/Hyland Point/Region_Downtown/Casino/casino/DoorWall (1)/OpeningHoursSign/Name";
            var gameObject = GameObject.Find(text);
            var flag2 = gameObject != null;
            if (flag2)
            {
                TextMeshPro component = gameObject.GetComponent<TextMeshPro>();
                var flag3 = component != null;
                if (flag3)
                {
                    component.m_text = "24/7";
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                }
                const string text2 = "Map/Hyland Point/Region_Downtown/Casino/casino/DoorWall/OpeningHoursSign/Name";
                var gameObject2 = GameObject.Find(text2);
                var flag4 = gameObject2 != null;
                if (flag4)
                {
                    var component2 = gameObject2.GetComponent<TextMeshPro>();
                    var flag5 = component2 != null;
                    if (flag5)
                    {
                        component2.m_text = "24/7";
                        gameObject2.SetActive(false);
                        gameObject2.SetActive(true);
                    }
                }
            }
        }
    }
}
