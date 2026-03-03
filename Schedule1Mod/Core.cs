using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts.Health;
using Schedule1Mod.Config;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(Schedule1Mod.Core), "Schedule1Mod", "1.0.0", "liam1581")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Schedule1Mod
{
    public class Core : MelonMod
    {
        public static SaveManager SaveMgr = new SaveManager();
        public static LoadManager LoadMgr = new LoadManager();
        public static PlayerHealth PlayerHlth = new PlayerHealth();


        private static MelonPreferences_Category _catSettings;

        private static MelonPreferences_Entry<string> _saveKey;
        private static MelonPreferences_Entry<string> _loadKey;
        private static MelonPreferences_Entry<string> _killKey;
        
        private static MelonPreferences_Entry<bool> _alwaysOpen;
        private static MelonPreferences_Entry<bool> _reqRank;
        private static MelonPreferences_Entry<string> _reqRankStr;
        
        public static MelonPreferences_Entry<bool> RestoreRv;
        
        private HarmonyLib.Harmony _harmony;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");

            _catSettings = MelonPreferences.CreateCategory("SaveMod", "Settings");
            _saveKey = _catSettings.CreateEntry("saveKeyBind", "F9");
            _loadKey = _catSettings.CreateEntry("loadKeyBind (EXPERIMENTAL!!!)", "");
            _killKey = _catSettings.CreateEntry("killKeyBind", "F10");
            RestoreRv = _catSettings.CreateEntry("Restore Already Exploded RV", false);
            _alwaysOpen = _catSettings.CreateEntry("AlwaysOpen", true, "If true, the warehouse is always open, even without completing the quest", oldIdentifier: null);
            _reqRank = _catSettings.CreateEntry("RequireRank", false, "If true, the player must have at least MinimumRank to open", oldIdentifier: null);
            _reqRankStr = _catSettings.CreateEntry("MinimumRank", "Street_Rat", "The minimum rank required to open the warehouse", oldIdentifier: null);
            
            ConfigManager.Config.AlwaysOpen = _alwaysOpen.Value;
            ConfigManager.Config.RequireRank = _reqRank.Value;
            ConfigManager.Config.MinimumRank = _reqRankStr.Value;

            LoggerInstance.Msg("Settings Loaded!");

            _harmony = new HarmonyLib.Harmony("com.leonyk2.schedule1mod");
            _harmony.PatchAll();
        }

        public override void OnUpdate()
        {
            if (Enum.TryParse(_saveKey.Value, out KeyCode keycode) && (_saveKey.Value != null || _saveKey.Value != ""))
            {
                if (Input.GetKeyDown(keycode))
                {
                    Util.Util.SaveGame();
                }
            }
            if (Enum.TryParse(_loadKey.Value, out KeyCode keycode1) && (_loadKey.Value != null || _loadKey.Value != ""))
            {
                if (Input.GetKeyDown(keycode1))
                {
                    Util.Util.LoadGame();
                }
            }
            if (Enum.TryParse(_killKey.Value, out KeyCode keycode2) && (_killKey.Value != null || _killKey.Value != ""))
            {
                if (Input.GetKeyDown(keycode2))
                {
                    Util.Util.KillPlayer();
                }
            }
        }
    }
}