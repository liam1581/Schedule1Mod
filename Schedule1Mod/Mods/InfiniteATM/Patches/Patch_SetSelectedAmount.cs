using HarmonyLib;
using Il2CppScheduleOne.UI.ATM;
using MelonLoader;
using Schedule1Mod.Util;
using UnityEngine.UI;


namespace Schedule1Mod.Mods.InfiniteATM.Patches;

[HarmonyPatch(typeof(ATMInterface), "SetSelectedAmount")]
public class Patch_SetSelectedAmount
{
    private const float MAX_SAFE_AMOUNT = 999000000f;

	public static Text cachedDisplayText = null;

	public static bool isInitializing = false;

	private static float pendingDisplayAmount = -1f;

	public static void Prefix(ATMInterface __instance, ref float amount)
	{
		try
		{
			pendingDisplayAmount = -1f;
			bool flag = false;
			foreach (Text componentsInChild in __instance.GetComponentsInChildren<Text>(true))
			{
				if (componentsInChild != null && componentsInChild.text?.ToLower().Contains("select amount to withdraw") == true)
				{
					flag = true;
					break;
				}
			}
			if (CashHelper.WasWithdrawalMode != flag)
			{
				CashHelper.AccumulatedAmount = 0f;
				CashHelper.LastSelectedAmount = 0f;
				CashHelper.LastButtonValue = 0f;
				CashHelper.LastWasMaxButton = false;
				CashHelper.WasWithdrawalMode = flag;
			}
			if (isInitializing && amount > 0f && amount <= 5000f)
			{
				amount = 0f;
				return;
			}
			float originalAmount = amount;
			if (flag)
			{
				ProcessWithdrawal(__instance, ref amount, originalAmount);
			}
			else
			{
				ProcessDeposit(__instance, ref amount, originalAmount);
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Error("[ATM] Error: " + ex.Message);
		}
	}

	private static bool IsSmallAdjustment(float amount, float accumulated)
	{
		if (accumulated == 0f)
		{
			return false;
		}
		float num = amount - accumulated;
		return num == 1f || num == -1f || num == 10f || num == -10f || num == 100f || num == -100f;
	}

	private static void ProcessIncrementDecrement(ATMInterface instance, ref float amount, float originalAmount, bool isWithdrawal)
	{
		float num = (isWithdrawal ? CashHelper.GetOnlineBalance(instance) : CashHelper.GetPlayerCash());
		if (originalAmount > num)
		{
			amount = num;
		}
		else
		{
			amount = originalAmount;
		}
		if (amount < 0f)
		{
			amount = 0f;
		}
		CashHelper.AccumulatedAmount = amount;
		CashHelper.LastSelectedAmount = amount;
		CashHelper.LastWasMaxButton = false;
		if (!isWithdrawal)
		{
			pendingDisplayAmount = amount;
		}
	}

	private static void ProcessWithdrawal(ATMInterface instance, ref float amount, float originalAmount)
	{
		if (IsSmallAdjustment(originalAmount, CashHelper.AccumulatedAmount))
		{
			ProcessIncrementDecrement(instance, ref amount, originalAmount, isWithdrawal: true);
		}
		else if (amount == 5000f)
		{
			float onlineBalance = CashHelper.GetOnlineBalance(instance);
			if (onlineBalance > 0f)
			{
				amount = ((onlineBalance > 999000000f) ? 999000000f : onlineBalance);
				UpdateState(amount, wasMaxButton: true, 5000f);
			}
		}
		else if (amount < 0f && !IsSmallAdjustment(originalAmount, CashHelper.AccumulatedAmount))
		{
			amount = CashHelper.LastWasMaxButton ? CashHelper.GetOnlineBalance(instance) : CashHelper.LastSelectedAmount > 0f ? CashHelper.LastSelectedAmount : 0f;
		}
		else if (amount > 0f)
		{
			float onlineBalance2 = CashHelper.GetOnlineBalance(instance);
			float num = CashHelper.MapButtonValue(originalAmount);
			if (CashHelper.LastWasMaxButton)
			{
				CashHelper.AccumulatedAmount = 0f;
			}
			CashHelper.LastWasMaxButton = false;
			float num2 = CashHelper.AccumulatedAmount + num;
			amount = num2 > onlineBalance2 ? onlineBalance2 : num2;
			CashHelper.AccumulatedAmount = amount;
			CashHelper.LastButtonValue = originalAmount;
			CashHelper.LastSelectedAmount = amount;
		}
	}

	private static void ProcessDeposit(ATMInterface instance, ref float amount, float originalAmount)
	{
		if (IsSmallAdjustment(originalAmount, CashHelper.AccumulatedAmount))
		{
			ProcessIncrementDecrement(instance, ref amount, originalAmount, isWithdrawal: false);
		}
		else if (amount == 10000f || amount < -100000000f)
		{
			float playerCash = CashHelper.GetPlayerCash();
			amount = ((playerCash > 999000000f) ? 999000000f : playerCash);
			UpdateState(amount, wasMaxButton: true, 10000f);
			pendingDisplayAmount = amount;
		}
		else if (amount == 5000f)
		{
			float playerCash2 = CashHelper.GetPlayerCash();
			float num = 5000f;
			if (CashHelper.LastWasMaxButton)
			{
				CashHelper.AccumulatedAmount = 0f;
			}
			CashHelper.LastWasMaxButton = false;
			float num2 = CashHelper.AccumulatedAmount + num;
			amount = ((num2 > playerCash2) ? playerCash2 : num2);
			CashHelper.AccumulatedAmount = amount;
			CashHelper.LastButtonValue = 100f;
			CashHelper.LastSelectedAmount = amount;
			pendingDisplayAmount = amount;
		}
		else if (amount < 0f && !IsSmallAdjustment(originalAmount, CashHelper.AccumulatedAmount))
		{
			amount = CashHelper.LastWasMaxButton ? CashHelper.GetPlayerCash() : CashHelper.LastSelectedAmount > 0f ? CashHelper.LastSelectedAmount : 0f;
			pendingDisplayAmount = amount;
		}
		else if (amount > 0f)
		{
			float playerCash3 = CashHelper.GetPlayerCash();
			float num3 = CashHelper.MapButtonValue(originalAmount);
			if (CashHelper.LastWasMaxButton)
			{
				CashHelper.AccumulatedAmount = 0f;
			}
			CashHelper.LastWasMaxButton = false;
			float num4 = CashHelper.AccumulatedAmount + num3;
			amount = num4 > playerCash3 ? playerCash3 : num4;
			CashHelper.AccumulatedAmount = amount;
			CashHelper.LastButtonValue = originalAmount;
			CashHelper.LastSelectedAmount = amount;
			pendingDisplayAmount = amount;
		}
	}

	private static void UpdateState(float amount, bool wasMaxButton, float buttonValue)
	{
		CashHelper.LastSelectedAmount = amount;
		CashHelper.LastWasMaxButton = wasMaxButton;
		CashHelper.LastButtonValue = buttonValue;
		CashHelper.AccumulatedAmount = amount;
	}

	public static void Postfix(ATMInterface __instance)
	{
		try
		{
			if (pendingDisplayAmount >= 0f)
			{
				UpdateDisplayText(__instance, pendingDisplayAmount);
				pendingDisplayAmount = -1f;
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Error("[ATM] Postfix error: " + ex.Message);
		}
	}

	private static void UpdateDisplayText(ATMInterface instance, float amount)
	{
		try
		{
			foreach (Text componentsInChild in instance.GetComponentsInChildren<Text>(true))
			{
				if (((componentsInChild != null) ? componentsInChild.text : null) == null)
				{
					continue;
				}
				if (componentsInChild.gameObject.name == "AmountText" || componentsInChild.text.Contains("-$"))
				{
					componentsInChild.text = $"${amount:N0}";
					if (cachedDisplayText == null)
					{
						cachedDisplayText = componentsInChild;
					}
				}
				else if (cachedDisplayText != null && componentsInChild == cachedDisplayText)
				{
					componentsInChild.text = $"${amount:N0}";
				}
			}
			if (cachedDisplayText != null && amount >= 0f)
			{
				cachedDisplayText.text = $"${amount:N0}";
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Error("[ATM] Display update error: " + ex.Message);
		}
	}
}