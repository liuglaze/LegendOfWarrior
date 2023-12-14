using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;
    public FadeEventSO fadeEventSO;
    private void OnEnable()
    {
        fadeEventSO.OnEventRaised += OnFadeEvent;
    }
    private void OnDisable()
    {
        fadeEventSO.OnEventRaised -= OnFadeEvent;
    }
    private void OnFadeEvent(Color target,float duration)
    {
        fadeImage.DOBlendableColor(target, duration);
    }
}
