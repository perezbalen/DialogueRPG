using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MakeCanvasInvisible : MonoBehaviour
{

    CanvasGroup canvasGroup;

    private float fadeInSpeed = 0.15f;
    private float fadeOutSpeed = 0.2f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        SwitchCanvasVisibility.OnAskedToHide += SetInvisible;
        SwitchCanvasVisibility.OnAskedToShow += SetVisible;
    }

    private void OnDisable()
    {
        SwitchCanvasVisibility.OnAskedToHide -= SetInvisible;
        SwitchCanvasVisibility.OnAskedToShow -= SetVisible;
    }

    public void SetInvisible()
    {
        canvasGroup.blocksRaycasts = false;

        LeanTween.cancel(gameObject);
        //canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasGroup, 0f, fadeOutSpeed);
    }

    public void SetVisible()
    {
        canvasGroup.blocksRaycasts = true;

        LeanTween.cancel(gameObject);
        //canvasGroup.alpha = 1f;
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
    }
}
