using Schedule1Mod.Util;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.UI.ATM;
using UnityEngine.UI;

namespace Schedule1Mod.Mods.InfiniteATM.Patches;

[HarmonyPatch(typeof(ATMInterface), "Update")]
public static class Patch_ATMUpdate
{
    private static ATMInterface lastInstance;
    private static bool hasInitialized;
    private static PropertyInfo selectedAmountProperty;
    private static Text maxButtonText;
    private static int framesSinceOpen;
    private static bool hasResetInitialAmount;
    
    public static void Postfix(ATMInterface __instance)
	{
		if (!__instance.isOpen)
		{
			if (lastInstance == __instance && hasInitialized)
			{
				ResetState();
			}
			return;
		}
		if (lastInstance != __instance || !hasInitialized)
		{
			InitializeState(__instance);
		}
		framesSinceOpen++;
		if (!hasResetInitialAmount && selectedAmountProperty != null && selectedAmountProperty.CanWrite)
		{
			ResetInitialAmount(__instance);
		}
		if (framesSinceOpen > 10)
		{
			Patch_SetSelectedAmount.isInitializing = false;
		}
		ATM.WEEKLY_DEPOSIT_LIMIT = 999000000f;
		ATM.DepositLimitEnabled = false;
		FixInvalidValues();
		UpdateButtonLabels(__instance);
	}

	private static void ResetState()
	{
		hasInitialized = false;
		maxButtonText = null;
		framesSinceOpen = 0;
		hasResetInitialAmount = false;
		Patch_SetSelectedAmount.cachedDisplayText = null;
		Patch_SetSelectedAmount.isInitializing = false;
		CashHelper.AccumulatedAmount = 0f;
		CashHelper.LastButtonValue = 0f;
		CashHelper.LastSelectedAmount = 0f;
		CashHelper.LastWasMaxButton = false;
		CashHelper.WasWithdrawalMode = false;
	}

	private static void InitializeState(ATMInterface instance)
	{
		lastInstance = instance;
		hasInitialized = true;
		framesSinceOpen = 0;
		hasResetInitialAmount = false;
		Patch_SetSelectedAmount.isInitializing = true;
		CashHelper.AccumulatedAmount = 0f;
		CashHelper.LastButtonValue = 0f;
		CashHelper.LastSelectedAmount = 0f;
		CashHelper.LastWasMaxButton = false;
		CashHelper.WasWithdrawalMode = false;
		if (selectedAmountProperty == null)
		{
			selectedAmountProperty = instance.GetType().GetProperty("selectedAmount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}
	}

	private static void ResetInitialAmount(ATMInterface instance)
	{
		bool wasWithdrawalMode = false;
		foreach (Text componentsInChild in instance.GetComponentsInChildren<Text>(true))
		{
			if (componentsInChild != null && componentsInChild.text?.ToLower().Contains("select amount to withdraw") == true)
			{
				wasWithdrawalMode = true;
				break;
			}
		}
		CashHelper.WasWithdrawalMode = wasWithdrawalMode;
		selectedAmountProperty.SetValue(instance, 0f);
		hasResetInitialAmount = true;
		CashHelper.AccumulatedAmount = 0f;
		CashHelper.LastSelectedAmount = 0f;
		CashHelper.LastButtonValue = 0f;
		CashHelper.LastWasMaxButton = false;
		foreach (Text componentsInChild2 in instance.GetComponentsInChildren<Text>(true))
		{
			if ((componentsInChild2 != null ? componentsInChild2.gameObject.name : null) == "AmountText")
			{
				componentsInChild2.text = "$0";
				Patch_SetSelectedAmount.cachedDisplayText = componentsInChild2;
				break;
			}
		}
	}

	private static void FixInvalidValues()
	{
		PropertyInfo propertyInfo = selectedAmountProperty;
		if (propertyInfo != null && propertyInfo.CanRead && selectedAmountProperty.CanWrite)
		{
			float num = (float)selectedAmountProperty.GetValue(lastInstance);
			if (num < 0f || float.IsNaN(num) || float.IsInfinity(num))
			{
				float num2 = CashHelper.LastSelectedAmount > 0f ? CashHelper.LastSelectedAmount : 0f;
				selectedAmountProperty.SetValue(lastInstance, num2);
			}
		}
	}

	private static void UpdateButtonLabels(ATMInterface instance)
	{
		bool flag = false;
		Il2CppArrayBase<Text> componentsInChildren = instance.GetComponentsInChildren<Text>(true);
		foreach (Text item in componentsInChildren)
		{
			if (item != null && item.text?.ToLower().Contains("select amount to withdraw") == true)
			{
				flag = true;
				break;
			}
		}
		foreach (Text item2 in componentsInChildren)
		{
			if ((item2 != null ? item2.text : null) == null)
			{
				continue;
			}
			if (item2.gameObject.name == "Text" && item2.text.StartsWith("$"))
			{
				Text val = item2;
				string text = item2.text;
				string text2 = text switch
				{
					"$20" => "$75", 
					"$50" => "$1K", 
					"$100" => "$5K", 
					"$500" => "$10K", 
					"$1,000" => "$50K", 
					"$5,000" => flag ? "WITHDRAW ALL" : item2.text, 
					_ => item2.text, 
				};
				val.text = text2;
			}
			if (item2.gameObject.name == "Text")
			{
				if (item2.text.Contains("$10,000") || item2.text.Contains("10,000") || (item2.text.Contains("ALL") && !item2.text.Contains("WITHDRAW")))
				{
					string text3 = flag ? "WITHDRAW ALL" : "DEPOSIT ALL";
					if (item2.text != text3)
					{
						item2.text = text3;
						if (maxButtonText == null)
						{
							maxButtonText = item2;
						}
					}
				}
				else
				{
					string text2 = item2.text;
					if (text2 == "WITHDRAW ALL" || text2 == "DEPOSIT ALL")
					{
						string text4 = flag ? "WITHDRAW ALL" : "DEPOSIT ALL";
						if (item2.text != text4)
						{
							item2.text = text4;
						}
					}
				}
			}
			else if (maxButtonText != null && item2 == maxButtonText)
			{
				string text5 = flag ? "WITHDRAW ALL" : "DEPOSIT ALL";
				if (item2.text != text5)
				{
					item2.text = text5;
				}
			}
			if (item2.gameObject.name == "AmountText" && item2.text.Contains("-$"))
			{
				float value = CashHelper.LastSelectedAmount > 0f ? CashHelper.LastSelectedAmount : 0f;
				item2.text = $"${value:N0}";
			}
		}
	}
}