using Cysharp.Threading.Tasks;
using Luck9kr.Uisystem;
using UnityEngine;
using UnityEngine.UI;


public class ColorMappingCaptureView : UIView
{
    Button captureBtn;
    bool bInit = false;
    ARCaptureViewStateType curState = ARCaptureViewStateType.Capture;

    public ARCaptureViewStateType PrevState { get; set; } = ARCaptureViewStateType.Play;
    public ARCaptureViewStateType CurState
    {
        get { return curState; }
        set
        {
            curState = value;
            SetState();
        }
    }


    public override void Show(object param = null)
    {
        ShowLayer();
        SetInfo().Forget();
    }

    protected override void OnFirstShow()
    {
        captureBtn = Find<Button>("CaptureState/CaptureBtn");
        captureBtn.onClick.AddListener(OnClick_CaptureBtn);
        Find<Button>("CaptureState/ExitBtn").onClick.AddListener(OnClick_ExitBtn);
        Find<Button>("ResultState/ExitBtn").onClick.AddListener(OnClick_ExitBtn);

    }

    protected override void OnEnableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }

    protected override void OnDisableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    void OnVuforiaStarted()
    {
        bInit = true;
    }

    async UniTask SetInfo()
    {
        CurState = ARCaptureViewStateType.Loading;
        await UniTask.WaitUntil(() => bInit);

        ColorMappingCaptureManager.Instance.Init(this);
        CurState = ARCaptureViewStateType.Capture;
    }

    void SetState()
    {
        Find("LoadingState").SetActive(curState == ARCaptureViewStateType.Loading);
        Find("CaptureState").SetActive(curState == ARCaptureViewStateType.Capture);
        Find("ResultState").SetActive(curState == ARCaptureViewStateType.Result);
    }

    public void SetState_Tracked(bool isTracked)
    {
        if (captureBtn == null)
            captureBtn = Find<Button>("CaptureState/CaptureBtn");

        captureBtn.interactable = isTracked;
    }

    void Update()
    {
        if (bInit)
            SetState_Tracked(ColorMappingCaptureManager.Instance.IsTargetTracked());
    }


    #region Button Event
    void OnClick_ExitBtn()
    {
        PopupState popup = My_UIManager.Instance.Popup<CommonMessagePopup>().Open(CommonPopupType.D, "Intro 화면으로 이동하시겠습니까?");
        popup.OnYes = p =>
        {
            Vuforia.VuforiaBehaviour.Instance.enabled = false;
            My_UIManager.Instance.Goto_IntroScene();
        };
    }

    void OnClick_CaptureBtn()
    {
        ColorMappingCaptureManager.Instance.SetColorMapping().Forget();
    }
    #endregion
}
