using Cysharp.Threading.Tasks;
using Luck9kr.Uisystem;
using UnityEngine;


public class SampleARCaptureView : UIView
{
    CustomButton captureBtn;
    bool bInit = false;



    public override void Show(object param = null)
    {
        ShowLayer();
    }

    protected override void OnFirstShow()
    {
        captureBtn = Find<CustomButton>("CaptureState/CaptureBtn");
        captureBtn.onClick.AddListener(OnClick_CaptureBtn);
        bInit = false;
    }

    protected override void OnEnableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        Find("LoadingState").SetActive(true);
        SetState_Capture(false);
    }

    protected override void OnDisableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    void OnVuforiaStarted()
    {
        SampleARCaptureManager.Instance.Init();
        SetState_Tracked(false);
        SetState_Capture(true);
        Find("LoadingState").SetActive(false);
        bInit = true;
    }

    public void SetState_Tracked(bool isTracked)
    {
        captureBtn.interactable = isTracked;
    }

    public void SetState_Capture(bool isOn)
    {
        Find("CaptureState").SetActive(isOn);
    }

    void Update()
    {
        if (bInit)
            SetState_Tracked(SampleARCaptureManager.Instance.IsTargetTracked());
    }


    #region Button Event
    void OnClick_ExitBtn()
    {
        Vuforia.VuforiaBehaviour.Instance.enabled = false;
        // WV_UIMamager.Instance.Goto_SquareScene();
    }

    void OnClick_CaptureBtn()
    {
        Find("CaptureState").SetActive(false);
        // Find("ExitBtn").SetActive(false);
        SampleARCaptureManager.Instance.SetColorMapping().Forget();
    }
    #endregion
}
