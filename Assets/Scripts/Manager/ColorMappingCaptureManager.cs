using Luck9kr.Uisystem;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;



public class ColorMappingCaptureManager : SingletonMonoBehaviour<ColorMappingCaptureManager>
{
    [SerializeField] int maxImageTargets = 1;
    [SerializeField] GameObject targetListObj;

    DefaultObserverEventHandler[] defaultObserverEventHandlers;
    Dictionary<string, bool> trackedStates = new Dictionary<string, bool>();
    ARLiveSketchColorMapping currentColorMapping;
    int arObjLayer;
    ColorMappingCaptureView view;



    public void Init(ColorMappingCaptureView view)
    {
        this.view = view;
        arObjLayer = LayerMask.NameToLayer("ARObj");
        trackedStates.Clear();
        Vuforia.VuforiaBehaviour.Instance.SetMaximumSimultaneousTrackedImages(maxImageTargets);
        defaultObserverEventHandlers = targetListObj.GetComponentsInChildren<DefaultObserverEventHandler>();
        foreach (DefaultObserverEventHandler handler in defaultObserverEventHandlers)
        {
            handler.OnTargetFound.AddListener(() => OnState_TargetFound(handler, true));
            handler.OnTargetLost.AddListener(() => OnState_TargetFound(handler, false));
        }
    }

    void OnDisable()
    {
        foreach (DefaultObserverEventHandler handler in defaultObserverEventHandlers)
        {
            handler.OnTargetFound.RemoveAllListeners();
            handler.OnTargetLost.RemoveAllListeners();
        }
    }

    public async UniTask SetColorMapping()
    {
        if (currentColorMapping != null)
        {
            Camera arCamera = Camera.main;
            int originalCullingMask = arCamera.cullingMask;

            try
            {
                arCamera.cullingMask &= ~(1 << arObjLayer);
                await UniTask.WaitForSeconds(0.5f);

                currentColorMapping.SetColorMapping();
            }
            finally
            {
                await UniTask.WaitForSeconds(0.5f);
                arCamera.cullingMask = originalCullingMask;
                view.CurState = ARCaptureViewStateType.Result;
            }
        }
    }

    public void OnState_TargetFound(DefaultObserverEventHandler handler, bool isTargetFound)
    {
        Vuforia.ImageTargetBehaviour imageTarget = handler.GetComponent<Vuforia.ImageTargetBehaviour>();
        if (!trackedStates.ContainsKey(imageTarget.TargetName))
            trackedStates.Add(imageTarget.TargetName, false);

        trackedStates[imageTarget.TargetName] = isTargetFound;

        if (isTargetFound)
        {
            currentColorMapping = handler.GetComponent<ARLiveSketchColorMapping>();
        }
    }

    public bool IsTargetTracked()
    {
        foreach (var kvp in trackedStates)
        {
            if (kvp.Value)
                return true;
        }

        return false;
    }
}
