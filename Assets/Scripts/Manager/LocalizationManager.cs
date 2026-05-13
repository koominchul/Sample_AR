using Cysharp.Threading.Tasks;
using Luck9kr.Uisystem;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;



public enum LanguageType { Korean = 0, English }

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{
    StringTable localizationTable = null;
    bool isChanging = false;
    const string tableName = "TextTable";

    public LanguageType CurrentLanguage
    {
        get
        {
            var locales = LocalizationSettings.AvailableLocales.Locales;
            var selected = LocalizationSettings.SelectedLocale;
            int index = locales.IndexOf(selected);
            return (LanguageType)index;
        }
    }


    protected override void OnAwakeSingleton()
    {
        base.OnAwakeSingleton();
        DontDestroyOnLoad(this);
    }

    public async UniTask Init()
    {
        await LocalizationSettings.InitializationOperation.ToUniTask();

        var tableOp = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
        await UniTask.WaitUntil(() => tableOp.IsDone);

        localizationTable = tableOp.Result;
    }

    public static string GetText(string key) => Instance.GetLocalizedString(key);
    public static void ChangeLanguage(int index) => Instance.ChangeLocale(index);

    string GetLocalizedString(string key)
    {
        var entry = localizationTable?.GetEntry(key);
        if (entry != null)
            return entry.GetLocalizedString();

        return key;
    }

    void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        _ChangeLocaleAsync(index).Forget();
    }

    async UniTask _ChangeLocaleAsync(int index)
    {
        isChanging = true;

        await LocalizationSettings.InitializationOperation.ToUniTask();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
}