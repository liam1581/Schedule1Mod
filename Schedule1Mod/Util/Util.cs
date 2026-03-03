using Il2CppScheduleOne;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Product;
using UnityEngine;

namespace Schedule1Mod.Util
{
    public static class Util
    {
        public static void SaveGame()
        {
            var flag = Core.SaveMgr == null;
            if (flag)
            {
                Core.SaveMgr = UnityEngine.Object.FindObjectOfType<SaveManager>();
            }
            var flag2 = Core.SaveMgr != null;
            if (flag2)
            {
                Core.SaveMgr.Save();
            }
        }

        public static void LoadGame()
        {
            var flag = Core.LoadMgr == null;
            if (flag)
            {
                Core.LoadMgr = UnityEngine.Object.FindObjectOfType<LoadManager>();
            }
            var flag2 = Core.LoadMgr != null;
            if (flag2)
            {
                Core.LoadMgr.LoadLastSave();
            }
        }

        public static void KillPlayer()
        {
            var flag = Core.PlayerHlth == null;
            if (flag)
            {
                Core.PlayerHlth = UnityEngine.Object.FindObjectOfType<Il2CppScheduleOne.PlayerScripts.Health.PlayerHealth>();
            }
            var flag2 = Core.PlayerHlth != null;
            if (flag2)
            {
                Core.PlayerHlth.Die();
            }
        }



        public static float EvaluateCounterofferPercentage(ProductDefinition product, int quantity, float price, Customer customer)
        {
            var adjustedWeeklySpend = customer.customerData.GetAdjustedWeeklySpend(customer.NPC.RelationData.RelationDelta / 5f);
            var orderDays = customer.customerData.GetOrderDays(customer.CurrentAddiction, customer.NPC.RelationData.RelationDelta / 5f);
            var num = adjustedWeeklySpend / orderDays.Count;
            if (price >= num * 3f)
            {
                return 0f;
            }
            var valueProposition = Customer.GetValueProposition(Registry.GetItem<ProductDefinition>(customer.OfferedContractInfo.Products.entries[0].ProductID), customer.OfferedContractInfo.Payment / customer.OfferedContractInfo.Products.entries[0].Quantity);
            var productEnjoyment = customer.GetProductEnjoyment(product, StandardsMethod.GetCorrespondingQuality(customer.customerData.Standards));
            var num2 = Mathf.InverseLerp(-1f, 1f, productEnjoyment);
            var valueProposition2 = Customer.GetValueProposition(product, price / quantity);
            var num3 = Mathf.Pow(quantity / (float)customer.OfferedContractInfo.Products.entries[0].Quantity, 0.6f);
            var num4 = Mathf.Lerp(0f, 2f, num3 * 0.5f);
            var num5 = Mathf.Lerp(1f, 0f, Mathf.Abs(num4 - 1f));
            if (valueProposition2 * num5 > valueProposition)
            {
                return 100f;
            }
            if (valueProposition2 < 0.12f)
            {
                return 0f;
            }
            var num6 = productEnjoyment * valueProposition;
            var num7 = num2 * num5 * valueProposition2;
            if (num7 > num6)
            {
                return 100f;
            }
            var num8 = num6 - num7;
            var num9 = Mathf.Lerp(0f, 1f, num8 / 0.2f);
            var num10 = Mathf.Max(customer.CurrentAddiction, customer.NPC.RelationData.NormalizedRelationDelta);
            var num11 = Mathf.Lerp(0f, 0.2f, num10);
            if (num9 <= num11)
            {
                return 100f;
            }
            if (num9 - num11 >= 0.9f)
            {
                return 0f;
            }
            return Mathf.Clamp((0.9f + num11 - num9) / 0.9f, 0f, 1f) * 100f;
        }
    }
}