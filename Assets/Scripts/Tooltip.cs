using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class Tooltip : MonoBehaviour
{
    #region Attrubutes
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI bodyField;
    public LayoutElement layoutElement;
    public int characterWrapLimit = 59;

    public RectTransform rectTransform;

    private CanvasGroup canvasGroup;

    [Header("Customization")]
    public float fadeInSpeed = 0.3f;
    public float fadeOutSpeed = 0.2f;

    private float pivotX;
    private float pivotY;

    [Header("Pointer offset")]
    public float xOffset = 0f;
    public float yOffset = 36f;

    #endregion

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    #region Position Tooltip

    /// <summary>
    /// Positions the tooltip within the screen
    /// </summary>
    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int bodyLength = bodyField.text.Length;
            layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit) ? true : false;
        }

        Vector2 position = Input.mousePosition;
    
        float boundryX = Screen.width - (rectTransform.rect.width); //Anchored bottom left to match pointer.
        float boundryY = rectTransform.rect.height; 

        /// Calculates the size of the tootip, (which needs a frame to set all things up) and then sets the pivots based on that.
        StartCoroutine(delayCalculateRectSize());

        if (position.x > boundryX + xOffset)
             pivotX = (position.x - boundryX ) / rectTransform.rect.width;
        if (position.y < boundryY + yOffset) 
            pivotY = (position.y - boundryY + rectTransform.rect.height ) / rectTransform.rect.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    /// <summary>
    /// Delays the calculation of the pivots by one frame
    /// So Unity has time to figure the size of the rectacngle
    /// </summary>
    /// <returns></returns>
    private IEnumerator delayCalculateRectSize()
    {
        yield return 0;

        // Normalize = (x - xMin) / (xMax - Xmin)
        // Normalize cuando min es 0 = x / xMax
        pivotX = xOffset / rectTransform.rect.width;
        pivotY = (rectTransform.rect.height + yOffset) / rectTransform.rect.height;
    }

    #endregion

    /// <summary>
    /// Whenver it's enabled, it triggers the fade in animation.
    /// </summary>
    public void FadeIn()
    {
        gameObject.SetActive(true);
        LeanTween.cancel(gameObject);
        canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInSpeed);
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        bodyField.text = content;
    }


    /// <summary>
    /// Fades out and then sends the order to SetActive(false)
    /// </summary>
    public void FadeOut()
    {
        if (canvasGroup != null)
        {
            //this sometimes calls an error. not sure why.
            canvasGroup.alpha = 1f;
            //Debug.Log("Fadeout w LeanTween.");
            LeanTween.alphaCanvas(canvasGroup, 0f, fadeOutSpeed)
                .setOnComplete(SetActiveToFalse);
            //SetText("Nothing.");
        }
        else
        {
            Debug.Log("Fadeout wothout LeanTween because canvasGroup is null.");
            SetActiveToFalse();
        }
    }

    /// <summary>
    /// SetActive to fasle, without doing animation 
    /// </summary>
    void SetActiveToFalse()
    {
        try
        {
            gameObject.SetActive(false);
        }
        catch (MissingReferenceException ex)
        {
            Debug.Log("Missing reference in HideMe.");
        }
    }


}