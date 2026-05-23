using HarmonyLib;
using Il2CppScheduleOne.Map;
using Il2CppTMPro;
using UnityEngine;

namespace Schedule1Mod.Mods.WarehouseAlwaysOpen.Patches;

[HarmonyPatch(typeof(TimedAccessZone), "Start")]
public class Patch_TimedAccessZone_DarkMarket
{
    [HarmonyPostfix]
    public static void Postfix(TimedAccessZone __instance)
    {
        var flag = __instance.gameObject.name.Equals("Casino");
        if (flag)
        {
            __instance.OpenTime = 500;
        }
        const string text = "Map/Hyland Point/Region_Docks/Dark Market Area/Docks Warehouse/dockswarehouse/Walls/OpeningHoursSign/Name";
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
        }
    }
}