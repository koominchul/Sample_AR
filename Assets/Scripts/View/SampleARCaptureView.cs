using Cysharp.Threading.Tasks;
using Luck9kr.Uisystem;
using UnityEngine;
using UnityEngine.UI;


public class SampleARCaptureView : UIView
{
    public enum ViewState { Loading, Capture, Play, Result }

    ViewState curState = ViewState.Capture;

    public ViewState PrevState { get; set; } = ViewState.Play;
    public ViewState CurState
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
        Find("LoadingState").SetActive(true);
        SetState_Capture(false);
    }

    protected override void OnDisableLayer()
    {
        Vuforia.VuforiaApplication.Instance.OnVuforiaStarted -= OnVuforiaStarted;
    }

    void OnVuforiaStarted()
    {
        SampleARCaptureManager.Instance.Init(this);
        SetState_Capture(true);
        Find("LoadingState").SetActive(false);
    }

    public void SetState_Capture(bool isOn)
    {
        Find("CaptureState").SetActive(isOn);
    }

    void SetState()
    {
        Find("LoadingState").SetActive(curState == ViewState.Loading);
        Find("CaptureState").SetActive(curState == ViewState.Capture);
        Find("PlayState").SetActive(curState == ViewState.Play);
        Find("ResultState").SetActive(curState == ViewState.Result);
    }


    #region Button Event
    void OnClick_ExitBtn()
    {
        // PopupState poupState = WV_UIMamager.Instance.Popup<CommonPopup>().Open(CommonPopupType.D, "creativepowerplan_goto_lobby", ePopupType.i_quit);
        // poupState.OnYes = p =>
        // {
        //     UIManager.Instance.LoadLevel(Constants.Scene.CreativePowerPlant_Lobby, null);
        // };
    }

    void OnClick_ReplayBtn()
    {
        CurState = ViewState.Play;
        SampleARCaptureManager.Instance.ReplayProduction();
    }
    #endregion
}
