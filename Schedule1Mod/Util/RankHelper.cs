using Il2CppScheduleOne.Levelling;
using Schedule1Mod.Config;
using Object = UnityEngine.Object;

namespace Schedule1Mod.Util
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

        public static bool HasRequiredRank()
        {
            if (!ConfigManager.Config.RequireRank)
            {
                return true; 
            }
            LevelManager lvlMan = Object.FindObjectOfType<LevelManager>();
            return lvlMan != null && lvlMan.Rank >= RankHelper.ParseRank(ConfigManager.Config.MinimumRank);
        }
    }
}
