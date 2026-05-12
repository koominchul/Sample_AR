using System.Collections;
using Luck9kr.Uisystem;
using UnityEngine;

public class My_UIManager : UIManager
{
    public static new My_UIManager Instance
    {
        get
        {
            if (UIManager.Instance == null)
                UIManager.Instance = FindAnyObjectByType<UIManager>();

            return UIManager.Instance as My_UIManager;
        }
    }
    public static object ViewParam { get { return viewParam; } }

    protected override void OnAwakeUI()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        base.OnAwakeUI();
    }

    protected override void OnDestroyUI()
    {

        base.OnDestroyUI();
    }

    protected override void OnBeginLoading()
    {
        Popup<LoadingPopup>(true).Open();
    }

    protected override void OnEndLoading()
    {
        Popup<LoadingPopup>().Close();
    }

    public T GetActivePopup<T>() where T : UIPopup
    {
        return (T)CurrentPopups.Find(p => p != null && p.gameObject.activeInHierarchy && p.name == typeof(T).Name);
    }

    public void SceneLoad(LoadingDataParam param)
    {
        StartCoroutine(SceneLoading(param));
    }

    public void Goto_IntroScene()
    {
        LoadLevel(Constants.Scene.Intro);
    }

    public IEnumerator SceneLoading(LoadingDataParam param)
    {
        yield return StartCoroutine(SceneLoadManager.Instance.FadeStart());
        LoadLevelAsync(Constants.Scene.Loaing, "LoadingView", param);
    }
}
