using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts.Health;
using UnityEngine;

namespace Schedule1Mod.Mods.Schedule1Mod;

public class Schedule1ModMod
{
    public static SaveManager SaveMgr;
    public static LoadManager LoadMgr;
    public static PlayerHealth PlayerHlth;
    
    public static void Update(Preferences prefs)
    {
        
        if (Enum.TryParse(prefs.KeybindSave.Value, out KeyCode saveKeycode) && (prefs.KeybindSave.Value != null || prefs.KeybindSave.Value != ""))
        {
            if (Input.GetKeyDown(saveKeycode))
            {
                SaveGame();
            }
        }
        if (Enum.TryParse(prefs.KeybindLoad.Value, out KeyCode loadKeycode) && (prefs.KeybindLoad.Value != null || prefs.KeybindLoad.Value != ""))
        {
            if (Input.GetKeyDown(loadKeycode))
            {
                LoadGame();
            }
        }
        if (Enum.TryParse(prefs.KeybindKill.Value, out KeyCode killKeycode) && (prefs.KeybindKill.Value != null || prefs.KeybindKill.Value != ""))
        {
            if (Input.GetKeyDown(killKeycode))
            {
                KillPlayer();
            }
        }
    }
    
    private static void SaveGame()
    {
        var flag = SaveMgr == null;
        if (flag)
        {
            SaveMgr = UnityEngine.Object.FindObjectOfType<SaveManager>();
        }
        var flag2 = SaveMgr != null;
        if (flag2)
        {
            SaveMgr.Save();
        }
    }

    private static void LoadGame()
    {
        var flag = LoadMgr == null;
        if (flag)
        {
            LoadMgr = UnityEngine.Object.FindObjectOfType<LoadManager>();
        }
        var flag2 = LoadMgr != null;
        if (flag2)
        {
            LoadMgr.LoadLastSave();
        }
    }

    private static void KillPlayer()
    {
        var flag = PlayerHlth == null;
        if (flag)
        {
            PlayerHlth = UnityEngine.Object.FindObjectOfType<Il2CppScheduleOne.PlayerScripts.Health.PlayerHealth>();
        }
        var flag2 = PlayerHlth != null;
        if (flag2)
        {
            PlayerHlth.Die();
        }
    }
}