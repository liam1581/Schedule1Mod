using MelonLoader;

namespace Schedule1Mod;

public class Preferences
{
    private readonly MelonPreferences_Category _businessIncomeSettings;
    private readonly MelonPreferences_Category _businessIncomeMultipliers;
    private readonly MelonPreferences_Category _docksWarehouseSettings;
    private readonly MelonPreferences_Category _saveModSettings;
    
    public MelonPreferences_Entry<bool> EnableNotifications { get; private set; }
    public MelonPreferences_Entry<float> MinBaseIncome { get; private set; }
    public MelonPreferences_Entry<float> MaxBaseIncome { get; private set; }
    public MelonPreferences_Entry<float> LaundromatMultiplier { get; private set; }
    public MelonPreferences_Entry<float> PostOfficeMultiplier { get; private set; }
    public MelonPreferences_Entry<float> CarWashMultiplier { get; private set; }
    public MelonPreferences_Entry<float> TacoTicklersMultiplier { get; private set; }
    
    public MelonPreferences_Entry<bool> AlwaysOpen { get; private set; }
    public MelonPreferences_Entry<bool> RequireRank { get; private set; }
    public MelonPreferences_Entry<string> MinimumRank { get; private set; }
    
    public MelonPreferences_Entry<string> KeybindSave { get; private set; }
    public MelonPreferences_Entry<string> KeybindLoad { get; private set; }
    public MelonPreferences_Entry<string> KeybindKill { get; private set; }

    public static readonly Dictionary<string, object> All = new();

    public Preferences()
    {
        _businessIncomeSettings = MelonPreferences.CreateCategory("BusinessIncome_Settings", "Settings");
        _businessIncomeMultipliers = MelonPreferences.CreateCategory("BusinessIncome_Multipliers", "Settings");

        EnableNotifications = AddEntry(_businessIncomeSettings, "EnableNotifications", true, "Enable Income Notifications", null);
        MinBaseIncome = AddEntry(_businessIncomeSettings, "MinBaseIncome", 100f, "Minimum Base Income", null);
        MaxBaseIncome = AddEntry(_businessIncomeSettings, "MaxBaseIncome", 300f, "Maximum Base Income", null);
        
        LaundromatMultiplier = AddEntry(_businessIncomeMultipliers, "LaundromatMultiplier", 1f, "Laundromat Multiplier", null);
        PostOfficeMultiplier = AddEntry(_businessIncomeMultipliers, "PostOfficeMultiplier", 1.5f, "Post Office Multiplier", null);
        CarWashMultiplier = AddEntry(_businessIncomeMultipliers, "CarWashMultiplier", 2f, "Car Wash Multiplier", null);
        TacoTicklersMultiplier = AddEntry(_businessIncomeMultipliers, "TacoTicklersMultiplier", 3f, "Taco Ticklers Multiplier", null);
        
        
        _docksWarehouseSettings = MelonPreferences.CreateCategory("DocksWarehouse_Settings", "Settings");
        
        AlwaysOpen = AddEntry(_docksWarehouseSettings, "AlwaysOpen", true, "If true, the warehouse is always open, even without completing the quest", null);
        RequireRank = AddEntry(_docksWarehouseSettings, "RequireRank", false, "If true, the player must have at least MinimumRank to open", null);
        MinimumRank = AddEntry(_docksWarehouseSettings, "MinimumRank", "Street_Rat", "The minimum rank to open the warehouse", null);
        
        
        _saveModSettings = MelonPreferences.CreateCategory("SaveMod_Settings", "Settings");

        KeybindSave = AddEntry(_saveModSettings, "SaveKey", "F9", "Keybinding for Saving the game", null);
        KeybindLoad = AddEntry(_saveModSettings, "LoadKey", "", "Keybind for Loading the last Save (NOT WORKING!!!)", null);
        KeybindKill = AddEntry(_saveModSettings, "KillKey", "F10", "Keybind for Killing the player", null);
    }

    public void SaveToFile()
    {
        _businessIncomeSettings.SaveToFile();
        _businessIncomeMultipliers.SaveToFile();
        _docksWarehouseSettings.SaveToFile();
        _saveModSettings.SaveToFile();
    }

    MelonPreferences_Entry<T> AddEntry<T>(MelonPreferences_Category cat, string id, T def, string dispName, string oldId)
    {
        All.Add(id, def);
        return cat.CreateEntry(id, def, dispName, oldIdentifier: oldId);
    }
}