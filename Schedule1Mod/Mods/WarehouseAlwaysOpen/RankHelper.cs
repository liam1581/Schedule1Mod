using Il2CppScheduleOne.Levelling;
using Object = UnityEngine.Object;

namespace Schedule1Mod.Mods.WarehouseAlwaysOpen
{
    public static class RankHelper
    {
        private static ERank ParseRank(string rank)
        {
            ERank result;
            try
            {
                result = (ERank)Enum.Parse(typeof(ERank), rank, true);
            } catch
            {
                return (ERank)3; 
            }
            return result;
        }

        public static bool HasRequiredRank(Preferences prefs)
        {
            if (!prefs.RequireRank.Value)
            {
                return true; 
            }
            LevelManager lvlMan = Object.FindObjectOfType<LevelManager>();
            return lvlMan != null && lvlMan.Rank >= RankHelper.ParseRank(prefs.MinimumRank.Value);
        }
    }
}
