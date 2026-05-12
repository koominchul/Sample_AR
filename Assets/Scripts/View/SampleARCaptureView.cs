using Luck9kr.Uisystem;
using UnityEngine.UI;


public class SampleARCaptureView : UIView
{


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
    }

    protected override void OnFirstShow()
    {
        Find<Button>("CaptureState/ExitBtn").onClick.AddListener(OnClick_ExitBtn);
        Find<Button>("PlayState/ExitBtn").onClick.AddListener(OnClick_ExitBtn);
        Find<Button>("ResultState/Buttons/ExitBtn").onClick.AddListener(OnClick_ExitBtn);
        Find<Button>("ResultState/Buttons/ReplayBtn").onClick.AddListener(OnClick_ReplayBtn);
    }

    protected override void OnEnableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
        CurState = ARCaptureViewStateType.Loading;
    }

    protected override void OnDisableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    void OnVuforiaStarted()
    {
        SampleARCaptureManager.Instance.Init(this);
        CurState = ARCaptureViewStateType.Capture;
    }

    void SetState()
    {
        Find("LoadingState").SetActive(curState == ARCaptureViewStateType.Loading);
        Find("CaptureState").SetActive(curState == ARCaptureViewStateType.Capture);
        Find("PlayState").SetActive(curState == ARCaptureViewStateType.Play);
        Find("ResultState").SetActive(curState == ARCaptureViewStateType.Result);
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

    void OnClick_ReplayBtn()
    {
        CurState = ARCaptureViewStateType.Play;
        SampleARCaptureManager.Instance.ReplayProduction();
    }
    #endregion
}
