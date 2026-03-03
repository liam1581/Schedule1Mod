using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Compass;
using MelonLoader;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Schedule1Mod.Patches
{
    [HarmonyPatch(typeof(CompassManager), "UpdateElements")]
    public static class Patch_CompassManager
    {
        private static void Postfix(CompassManager __instance)
        {
            try
            {
                foreach (CompassManager.Element element in __instance.elements)
                {
                    bool flag = element.Visible && element.Transform != null;
                    if (flag)
                    {
                        float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, element.Transform.position);
                        element.DistanceLabel.text = Mathf.CeilToInt(num).ToString() + "m";
                    }
                }
            }
            catch (Exception value)
            {
                var logger = Melon<Core>.Logger;
                var defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                defaultInterpolatedStringHandler.AppendLiteral("error: ");
                defaultInterpolatedStringHandler.AppendFormatted(value);
                logger.Msg(defaultInterpolatedStringHandler.ToStringAndClear());
            }
        }
    }
}
