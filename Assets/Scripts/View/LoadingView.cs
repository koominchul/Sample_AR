using System.Collections;
using Luck9kr.Uisystem;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingView : UIView
{
    [SerializeField] float delay = 1.5f;

    LoadingDataParam loadingDataParam;


    public override void Show(object param = null)
    {
        if (param != null)
            loadingDataParam = (LoadingDataParam)param;

        ShowLayer();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return StartCoroutine(SceneLoadManager.Instance.FadeEnd());
        yield return new WaitForSeconds(delay);

        bool bStartFade = false;
        AsyncOperation async = My_UIManager.Instance.LoadLevelAsync(loadingDataParam.loadScene, loadingDataParam.viewName, loadingDataParam, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                if (!bStartFade)
                {
                    yield return StartCoroutine(SceneLoadManager.Instance.FadeStart());
                    async.allowSceneActivation = true;
                    bStartFade = true;
                }
            }

            yield return null;
        }

        SceneManager.UnloadSceneAsync(Constants.Scene.Loaing);
        StartCoroutine(SceneLoadManager.Instance.FadeEnd());
    }
}
