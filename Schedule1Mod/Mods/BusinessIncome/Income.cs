using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Property;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Schedule1Mod.Mods.BusinessIncome;

internal sealed class Income(Preferences prefs)
{
    private readonly Sprite _cashFront = FindCashSprite();
    private readonly Preferences _prefs = prefs;

    public void TriggerPayouts()
    {
        foreach (Business business in Business.OwnedBusinesses)
        {
            int income = CalculateIncome(business);
            if (_prefs.EnableNotifications.Value)
            {
                string money = MoneyManager.ApplyMoneyTextColor($"${income}");
                Managers.Notifications.SendNotification(business.name, $"Made {money} today", _cashFront);
            }
            Managers.Money.CreateOnlineTransaction(business.propertyName, income, 1, "Income");
        }
    }

    private int CalculateIncome(Business business)
    {
        float minIncome = _prefs.MinBaseIncome.Value;
        float maxIncome = _prefs.MaxBaseIncome.Value;
        float multiplier = GetIncomeMultiplier(business.PropertyName);
        return Mathf.RoundToInt(Random.Range(minIncome, maxIncome) * multiplier);
    }

    private float GetIncomeMultiplier(string businessName)
    {
        return businessName switch
        {
            "Laundromat" => _prefs.LaundromatMultiplier.Value,
            "Post Office" => _prefs.PostOfficeMultiplier.Value,
            "Car Wash" => _prefs.CarWashMultiplier.Value,
            "Taco Ticklers" => _prefs.TacoTicklersMultiplier.Value,
            _ => 1f,
        };
    }

    private static Sprite FindCashSprite()
    {
        Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == "cash_front")
            {
                return sprite;
            }
        }

        return null;
    }
}