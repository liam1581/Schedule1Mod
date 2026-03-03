using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Property;
using MelonLoader;
using UnityEngine;

namespace Schedule1Mod.Patches;

[HarmonyPatch(typeof(RV))]
public class Patch_RV
{
    [HarmonyPatch("SetExploded", new Type[]
    {

    })]
    [HarmonyPrefix]
    public static bool Prefix(RV __instance)
    {
        bool flag = !Singleton<LoadManager>.Instance.IsGameLoaded;
        bool result;
        if (flag)
        {
            MelonLogger.Msg("Save setting value before game loaded");
            result = !Core.RestoreRv.Value;
        }
        else
        {
            MelonLogger.Msg("SetExploded function call skipped");
            GameObject childGameObject = GetChildGameObject(__instance.gameObject, "Destroyed RV");
            bool flag2 = childGameObject != null;
            if (flag2)
            {
                childGameObject.SetActive(true);
                GameObject childGameObject2 = GetChildGameObject(childGameObject, "CartelNote");
                bool flag3 = childGameObject2 != null;
                if (flag3)
                {
                    MelonLogger.Msg("Making Cartel note visible");
                    MelonLogger.Msg("Enjoy your game session with your RV");
                    childGameObject2.SetActive(true);
                }
                GameObject childGameObject3 = GetChildGameObject(childGameObject, "destroyed rv");
                bool flag4 = childGameObject3 != null;
                if (flag4)
                {
                    childGameObject3.SetActive(false);
                }
            }
            else
            {
                MelonLogger.Msg("Child object not found.");
            }
            result = false;
        }
        return result;
    }
    
    private static GameObject GetChildGameObject(GameObject obj, string childName)
    {
        Transform transform = obj.transform.Find(childName);
        bool flag = transform != null;
        GameObject result;
        if (flag)
        {
            result = transform.gameObject;
        }
        else
        {
            result = null;
        }
        return result;
    }
}