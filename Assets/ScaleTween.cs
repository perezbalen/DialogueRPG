using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTween : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration;
    public float delay;
    public LeanTweenType easeType;

    public UnityEvent onCompleteCallback;

    public void OnEnable()
    {
        //transform.localScale = new Vector3(0, 0, 0); // this makes the thig size 0.
        //LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f).setDelay(2.1f).setOnComplete(OnComplete);

        gameObject.LeanAlpha(0, 0.5f);
        if (easeType == LeanTweenType.animationCurve)
        {
            LeanTween.alpha(gameObject, 255,delay).setDelay(delay).setEase(curve);
            //scale(gameObject, new Vector3(1, 1, 1), duration).setDelay(delay).setEase(curve); 
        }
        else
        {
            LeanTween.alpha(gameObject, 255, delay).setDelay(delay).setEase(easeType);
            //LeanTween.scale(gameObject, new Vector3(1, 1, 1), duration).setDelay(delay).setEase(easeType);
        }
    }

    public void OnComplete()
    {
        if (onCompleteCallback != null)
        {
            onCompleteCallback.Invoke();
        }
    }

    //when the close button is pressed.
    public void OnClose()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(DestroyMe);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
