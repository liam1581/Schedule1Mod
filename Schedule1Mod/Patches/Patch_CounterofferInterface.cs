using HarmonyLib;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.UI.Phone;
using UnityEngine;
using UnityEngine.UI;

namespace Schedule1Mod.Patches
{
    [HarmonyPatch(typeof(CounterofferInterface))]
    public static class Patch_CounterofferInterface
    {
        private static void UpdateConfirmButtonText(CounterofferInterface instance)
        {
            var componentInChildren = instance.ConfirmButton.GetComponentInChildren<Text>();
            var num = Util.Util.EvaluateCounterofferPercentage(instance.selectedProduct, instance.quantity, instance.price, instance.conversation.sender.GetComponent<Customer>());
            componentInChildren.text = string.Format("Send ({0}%)", Mathf.RoundToInt(num));
        }

        [HarmonyPatch("Open"), HarmonyPostfix]
        private static void PostOpenPatch(CounterofferInterface __instance)
        {
            UpdateConfirmButtonText(__instance);
        }

        [HarmonyPatch("ChangePrice"), HarmonyPostfix]
        private static void PostPriceChangePatch(CounterofferInterface __instance)
        {
            UpdateConfirmButtonText(__instance);
        }

        [HarmonyPatch("ChangeQuantity"), HarmonyPostfix]
        private static void PostQuantityChangePatch(CounterofferInterface __instance)
        {
            UpdateConfirmButtonText(__instance);
        }
    }
}
