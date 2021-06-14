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

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetVisible()
    {
        canvasGroup.blocksRaycasts = true;
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
    }


    public void SetInvisible()
    {
        StartCoroutine(WaitToHide());    
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
