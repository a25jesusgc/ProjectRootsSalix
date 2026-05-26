using UnityEngine;
using UnityEngine.Localization.Settings;

public class GlobalUtils : MonoBehaviour
{
    public static bool pause = false;
    public static string SAVE_PATH = Application.persistentDataPath + "/save.json";

    public static string GetLocalizedString(string table, string key)
    {
        return LocalizationSettings.StringDatabase.GetTable(table).GetEntry(key).GetLocalizedString(null);
    }
    public static string GetLocalizedString(string table, string key, object[] args)
    {
        return LocalizationSettings.StringDatabase.GetTable(table).GetEntry(key).GetLocalizedString(args);
    }
}
