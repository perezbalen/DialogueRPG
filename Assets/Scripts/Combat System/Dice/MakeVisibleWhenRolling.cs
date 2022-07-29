using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MakeVisibleWhenRolling : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    
    public float fadeInSpeed = 0.3f;
    public float fadeOutSpeed = 1.7f;
    public float waitBeforeFadeOut = 5f;

    private Coroutine w;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; //make invisible when game starts.
    }

    public void SetVisible()
    {
        canvasGroup.alpha = 0; // so that it allways fades in.
        canvasGroup.blocksRaycasts = true;
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
    }


    public void SetInvisible()
    {
        if (w != null)
            StopCoroutine(w);
        w = StartCoroutine(WaitToHide());    
    }

    /// <summary>
    /// another thread to not block Unity
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToHide()
    {
        yield return new WaitForSeconds(waitBeforeFadeOut);
        canvasGroup.blocksRaycasts = false;
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(canvasGroup, 0f, fadeOutSpeed);
    }



}
