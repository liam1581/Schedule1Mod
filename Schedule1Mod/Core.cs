using MelonLoader;

using Schedule1Mod.Mods.BusinessIncome;
using Schedule1Mod.Mods.InfiniteATM;
using Schedule1Mod.Mods.Schedule1Mod;

[assembly: MelonInfo(typeof(Schedule1Mod.Core), "Schedule1Mod", "1.0.0", "liam1581")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Schedule1Mod
{
    public class Core : MelonMod
    {
        private Preferences _prefs = new();
        
        private HarmonyLib.Harmony _harmony;
        
        public static MelonLogger.Instance Logger;

        public override void OnInitializeMelon()
        {
            Logger = LoggerInstance;
            LoggerInstance.Msg("Initialized.");

            BusinessIncome.Init(_prefs);
            
            _harmony = new HarmonyLib.Harmony("com.leonyk2.schedule1mod");
            _harmony.PatchAll();
        }

        public override void OnDeinitializeMelon()
        {
            _prefs.SaveToFile();
        }

        public override void OnUpdate()
        {
            InfiniteATM.Update();
            Schedule1ModMod.Update(_prefs);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            BusinessIncome.SceneWasInitialized(buildIndex, sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            BusinessIncome.SceneWasUnloaded(buildIndex, sceneName);
        }
    }
}