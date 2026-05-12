
using LitMotion;
using LitMotion.Extensions;
using Luck9kr.Uisystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LitMotion_TestView : UIView
{
    [SerializeField] TextMeshPro tmpText;
    [SerializeField] TextMeshProUGUI tmpUGuiText;
    [SerializeField] Gradient colorGradient;



    public override void Show(object param = null)
    {
        ShowLayer();
    }

    protected override void OnFirstShow()
    {
        Find<Button>("TMPAnimationBtn").onClick.AddListener(OnClick_TMPAnimationBtn);
    }


    #region Button Event
    private void OnClick_TMPAnimationBtn()
    {
        if (tmpText != null)
        {
            Vector3 strength = Vector3.up * 0.3f;
            for (int i = 0; i < tmpText.textInfo.characterCount; i++)
            {
                LMotion.Create(0f, 1f, 3f)
                    .WithDelay(i * 0.1f)
                    .WithEase(Ease.OutQuad)
                    .WithLoops(-1, LoopType.Yoyo)
                    .BindToTMPChar(tmpText, i, (float t, int idx, ref TMPMotionCharacter ch) =>
                    {
                        ch.Color = colorGradient.Evaluate(t);
                    });

                LMotion.Punch.Create(Vector3.zero, strength, 3f)
                    .WithDelay(i * 0.1f)
                    .WithEase(Ease.Linear)
                    .WithLoops(-1, LoopType.Restart)
                    .BindToTMPCharPosition(tmpText, i);
            }
        }

        if (tmpUGuiText != null)
        {
            Vector3 strength = Vector3.up * 10f;
            for (int i = 0; i < tmpUGuiText.textInfo.characterCount; i++)
            {
                LMotion.Create(0f, 1f, 3f)
                    .WithDelay(i * 0.1f)
                    .WithEase(Ease.OutQuad)
                    .WithLoops(-1, LoopType.Yoyo)
                    .BindToTMPChar(tmpUGuiText, i, (float t, int idx, ref TMPMotionCharacter ch) =>
                    {
                        ch.Color = colorGradient.Evaluate(t);
                    });

                LMotion.Punch.Create(Vector3.zero, strength, 3f)
                    .WithDelay(i * 0.1f)
                    .WithEase(Ease.Linear)
                    .WithLoops(-1, LoopType.Restart)
                    .BindToTMPCharPosition(tmpUGuiText, i);
            }
        }
    }
    #endregion
}
