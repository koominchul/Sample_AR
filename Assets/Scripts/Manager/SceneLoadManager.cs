using System.Collections;
using UnityEngine;
using Coffee.UIEffects;
using Luck9kr.Uisystem;


public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{
    [SerializeField] Texture2D[] textures;
    [SerializeField] UIEffect uiEffect;
    [SerializeField] UIEffectTweener uiTweener;
    [SerializeField] float transitionTime = 1f;


    protected override void OnInitialize()
    {
        uiEffect.transitionRate = 1f;
        uiEffect.graphic.raycastTarget = false;

        uiTweener.duration = transitionTime;
        uiTweener.wrapMode = UIEffectTweener.WrapMode.Once;
        uiTweener.playOnEnable = UIEffectTweener.PlayOnEnable.None;
    }

    protected override void OnAwakeSingleton()
    {
        base.OnAwakeSingleton();
        DontDestroyOnLoad(this);
    }

    public IEnumerator FadeStart()
    {
        uiEffect.transitionTexture = textures[Random.Range(0, textures.Length)];
        uiTweener.PlayReverse(true);
        yield return new WaitUntil(() => !uiTweener.isTweening);
    }

    public IEnumerator FadeEnd()
    {
        uiEffect.transitionTexture = textures[Random.Range(0, textures.Length)];
        uiTweener.PlayForward(true);
        yield return new WaitUntil(() => !uiTweener.isTweening);
    }
}
