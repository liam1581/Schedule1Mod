namespace Schedule1Mod.Mods.BusinessIncome;

public class BusinessIncome
{
    private static Action _onMinutePass;
    private static Preferences _prefs;
    private static Income _income;
    
    public static void Init(Preferences prefs)
    {
        _prefs = prefs;
    }

    public static void SceneWasInitialized(int buildIndex, string sceneName)
    {
        if (sceneName != "Main" || !Managers.Get()) return;

        _income ??= new Income(_prefs);

        _onMinutePass = OnMinutePass;
        Managers.Time.onMinutePass.Add(_onMinutePass);
    }

    public static void SceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (!Managers.IsInitialized) return;

        Managers.Time.onMinutePass.Remove(_onMinutePass);
        _onMinutePass = null;
    }
    
    private static void OnMinutePass()
    {
        if (Managers.Time.CurrentTime != 1759) return;

        Core.Logger.Msg("Payout time!");
        _income.TriggerPayouts();
    }
}