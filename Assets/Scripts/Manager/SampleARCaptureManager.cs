using Luck9kr.Uisystem;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.InputSystem;



public class SampleARCaptureManager : SingletonMonoBehaviour<SampleARCaptureManager>
{
    [SerializeField] InputActionReference dragAction;
    [SerializeField] int maxImageTargets = 1;
    [SerializeField] float rotationSpeed = 0.05f;
    [SerializeField] GameObject targetListObj;

    DefaultObserverEventHandler[] defaultObserverEventHandlers;
    Dictionary<string, bool> trackedStates = new Dictionary<string, bool>();
    SampleARCaptureView view;
    PlayableDirector currentPD;
    string currentTargetName = "";
    GameObject currentHandleObj;
    bool isPressing = false;



    public void Init(SampleARCaptureView view)
    {
        this.view = view;
        trackedStates.Clear();

        Vuforia.VuforiaBehaviour.Instance.SetMaximumSimultaneousTrackedImages(maxImageTargets);

        defaultObserverEventHandlers = targetListObj.GetComponentsInChildren<DefaultObserverEventHandler>();
        foreach (DefaultObserverEventHandler handler in defaultObserverEventHandlers)
        {
            handler.OnTargetFound.AddListener(() => OnState_TargetFound(handler, true));
            handler.OnTargetLost.AddListener(() => OnState_TargetFound(handler, false));
        }
    }

    void OnEnable()
    {
        dragAction.action.started += OnDragStarted;
        dragAction.action.canceled += OnDragCanceled;
        dragAction.action.performed += OnDragPerformed;
    }

    void OnDisable()
    {
        dragAction.action.started -= OnDragStarted;
        dragAction.action.canceled -= OnDragCanceled;
        dragAction.action.performed -= OnDragPerformed;

        if (Vuforia.VuforiaBehaviour.Instance != null)
            Vuforia.VuforiaBehaviour.Instance.enabled = false;

        foreach (DefaultObserverEventHandler handler in defaultObserverEventHandlers)
        {
            handler.OnTargetFound.RemoveAllListeners();
            handler.OnTargetLost.RemoveAllListeners();
        }
    }

    public void ReplayProduction()
    {
        currentPD.stopped += OnTimelineStopped;
        currentPD.time = 0;
        currentPD.Play();
    }


    #region Event
    void OnState_TargetFound(DefaultObserverEventHandler handler, bool isTargetFound)
    {
        Vuforia.ImageTargetBehaviour imageTarget = handler.GetComponent<Vuforia.ImageTargetBehaviour>();
        if (!trackedStates.ContainsKey(imageTarget.TargetName))
            trackedStates.Add(imageTarget.TargetName, false);

        trackedStates[imageTarget.TargetName] = isTargetFound;
        if (isTargetFound)
        {
            PlayableDirector pd = handler.GetComponentInChildren<PlayableDirector>();
            if (pd != null)
            {
                view.CurState = ARCaptureViewStateType.Play;
                currentHandleObj = handler.transform.GetChild(0).gameObject;
                if (currentTargetName != imageTarget.TargetName)
                {
                    view.PrevState = ARCaptureViewStateType.Play;
                    if (currentPD != null)
                        currentPD.stopped -= OnTimelineStopped;

                    currentTargetName = imageTarget.TargetName;
                    currentPD = pd;
                    currentPD.stopped += OnTimelineStopped;
                    currentPD.time = 0;
                    currentPD.Play();
                }
                else
                {
                    if (view.PrevState == ARCaptureViewStateType.Result)
                    {
                        view.CurState = ARCaptureViewStateType.Result;
                    }

                    if (currentPD.state == PlayState.Paused)
                        currentPD.Resume();
                }
            }
        }
        else
        {
            currentHandleObj = null;
            if (currentPD != null)
            {
                if (currentPD.state != PlayState.Paused)
                    currentPD.Pause();
            }

            view.CurState = ARCaptureViewStateType.Capture;
        }
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        director.stopped -= OnTimelineStopped;
        if (My_UIManager.Instance != null)
            view.CurState = ARCaptureViewStateType.Result;
    }

    void OnDragStarted(InputAction.CallbackContext context)
    {
        isPressing = true;
    }

    void OnDragCanceled(InputAction.CallbackContext context)
    {
        isPressing = false;
    }

    void OnDragPerformed(InputAction.CallbackContext context)
    {
        if (isPressing && currentHandleObj != null)
        {
            Vector2 dragDelta = context.ReadValue<Vector2>();
            float rotationY = dragDelta.x * rotationSpeed;
            currentHandleObj.transform.Rotate(0, -rotationY, 0, Space.Self);
        }
    }
    #endregion
}
