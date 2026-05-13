using Cysharp.Threading.Tasks;
using Luck9kr.Uisystem;
using UnityEngine.UI;

public class IntroView : UIView
{


    public override void Show(object param = null)
    {
        ShowLayer();
        SetInfo().Forget();
    }

    protected override void OnFirstShow()
    {
        Find<Button>("Buttons/SampleARBtn").onClick.AddListener(OnClick_SampleARBtn);
        Find<Button>("Buttons/ColorMappingBtn").onClick.AddListener(OnClick_ColorMappingBtn);
    }

    async UniTask SetInfo()
    {
        await UniTask.WaitUntil(() => LocalizationManager.IsLoadCompleted);
        await LocalizationManager.Instance.Init();
    }


    #region Event
    void OnClick_SampleARBtn()
    {
        My_UIManager.Instance.SceneLoad(new LoadingDataParam()
        {
            loadScene = Constants.Scene.SampleAR,
            viewName = typeof(SampleARCaptureView).Name,
            param = null
        });
    }

    void OnClick_ColorMappingBtn()
    {
        My_UIManager.Instance.SceneLoad(new LoadingDataParam()
        {
            loadScene = Constants.Scene.ColorMapping,
            viewName = typeof(ColorMappingCaptureView).Name,
            param = null
        });
    }
    #endregion
}
