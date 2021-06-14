using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

/// <summary>
/// Tooltip with combat information taken from the card
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CardInfo : MonoBehaviour
{
    #region Paramenters 
    [TextArea]
    public string bodyMessage;
    private TextMeshProUGUI body;

    public CardDisplay cardDisplay;
    public bool hasCardData = false; //does the latest dialoge option has Card Data to display? This is set by the TooltioForDialogueSystem.

    [Header("Inner Workings")]
    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [Header("Customization")]
    public float fadeInSpeed = 0.03f;
    public float fadeOutSpeed = 0.2f;

    private float pivotX;
    private float pivotY;

    [Header("Pointer offset")]
    public float xOffset = 0f;
    public float yOffset = 36f;
    public bool upperAnchor = false;
    #endregion

    #region Card infromation TMP
    [Header("Connect to itself")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI dificulty;
    public TextMeshProUGUI stat;
    public TextMeshProUGUI successPlayer1;
    public TextMeshProUGUI successPlayer2;
    public TextMeshProUGUI failurePlayer1;
    public TextMeshProUGUI failurePlayer2;

    public Image backStamina;
    public Image backTechnique;
    public Image backCharisma;
    public Image backWill;

    #endregion

    #region Setup
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        body = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        body.text = bodyMessage;
    }

    // Update is called once per frame
    void OnValidate()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;

        body = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        body.text = bodyMessage;
    }
    #endregion

    public void ShowCardStats()
    {
        if (cardDisplay != null && hasCardData) //only do this if card exists, and it's flagged to show data.
        {
            //bodyMessage = "<b>" + cardDisplay.currentCard.cardName + "</b><br>";
            fillTitle(cardDisplay.currentCard.name);
            fillDificulty(cardDisplay.currentCard.cardDificulty.ToString());
            fillStat(cardDisplay.currentCard.statDependency.ToString());
            //bodyMessage += "<b>" + cardDisplay.currentCard.statDependency + "</b> check.<br>";
            //bodyMessage += "Dificulty: " + cardDisplay.currentCard.cardDificulty + ".<br> Needs a 5 or more.<br>";
            //bodyMessage += "Success means: <br><b>Exhaustion: " + cardDisplay.currentCard.exhaustionHitPlayer1Success + "</b><br>";
            fillSuccessPlayer1(cardDisplay.currentCard);
            fillSuccessPlayer2(cardDisplay.currentCard);
            fillFailurePlayer1(cardDisplay.currentCard);
            fillFailurePlayer2(cardDisplay.currentCard);
            //body.text = bodyMessage;

            FadeIn();
            //canvasGroup.alpha = 1f;
        }
        else
            HideCardStats();
    }

    #region Fill the info texts
    private void fillTitle(string newTitle)
    {
        title.text = newTitle;
    }

    private void fillDificulty (string newDificulty)
    {
        if (newDificulty ==         CardObject.CardDificulty.Easy.ToString())
            dificulty.text = "It's an <b>easy</b> action. You need to roll 5 or hihger.";
        else if (newDificulty ==    CardObject.CardDificulty.Medium.ToString())
            dificulty.text = "It's a <b>medium hard</b> action. You need to roll 7 or hihger.";
        else if (newDificulty ==    CardObject.CardDificulty.Hard.ToString())
            dificulty.text = "It's a <b>hard</b> action. You need to roll 9 or hihger.";
        else
            dificulty.text = "SOMETHING WENT WRONG. Can't find hardness.";
    }

    private void fillStat(string newStat)
    {
        stat.text = "The modifier depends on your <b>" + newStat +  "</b>";
    }

    private void fillSuccessPlayer1(Card card)
    {
        successPlayer1.text = "";
        if (card.exhaustionHitPlayer1Success!=0 || card.frustrationHitPlayer1Success != 0 || card.arousalHitPlayer1Success != 0)
            successPlayer1.text += "<b>For you:</b><br>";
        if (card.exhaustionHitPlayer1Success > 0)
            successPlayer1.text += "Exhaustion +" + card.exhaustionHitPlayer1Success + "<br>";
        if (card.exhaustionHitPlayer1Success < 0)
            successPlayer1.text += "Exhaustion " + card.exhaustionHitPlayer1Success + "<br>";
        if (card.frustrationHitPlayer1Success > 0)
            successPlayer1.text += "Frustration +" + card.frustrationHitPlayer1Success + "<br>";
        if (card.frustrationHitPlayer1Success < 0)
            successPlayer1.text += "Frustration " + card.frustrationHitPlayer1Success + "<br>";
        if (card.arousalHitPlayer1Success > 0)
            successPlayer1.text += "Arousal +" + card.arousalHitPlayer1Success + "<br>";
        if (card.arousalHitPlayer1Success < 0)
            successPlayer1.text += "Arousal " + card.arousalHitPlayer1Success + "<br>";
    }

    private void fillSuccessPlayer2(Card card)
    {
        successPlayer2.text = "";
        if (card.exhaustionHitPlayer2Success != 0 || card.frustrationHitPlayer2Success != 0 || card.arousalHitPlayer2Success != 0)
            successPlayer2.text += "<b>For her:</b><br>";
        if (card.exhaustionHitPlayer2Success > 0)
            successPlayer2.text += "Exhaustion +" + card.exhaustionHitPlayer2Success + "<br>";
        if (card.exhaustionHitPlayer2Success < 0)
            successPlayer2.text += "Exhaustion " + card.exhaustionHitPlayer2Success + "<br>";
        if (card.frustrationHitPlayer2Success > 0)
            successPlayer2.text += "Frustration +" + card.frustrationHitPlayer2Success + "<br>";
        if (card.frustrationHitPlayer2Success < 0)
            successPlayer2.text += "Frustration " + card.frustrationHitPlayer2Success + "<br>";
        if (card.arousalHitPlayer2Success > 0)
            successPlayer2.text += "Arousal +" + card.arousalHitPlayer2Success + "<br>";
        if (card.arousalHitPlayer2Success < 0)
            successPlayer2.text += "Arousal " + card.arousalHitPlayer2Success + "<br>";
    }

    private void fillFailurePlayer1(Card card)
    {
        failurePlayer1.text = "";
        if (card.exhaustionHitPlayer1Failure != 0 || card.frustrationHitPlayer1Failure != 0 || card.arousalHitPlayer1Failure != 0)
            failurePlayer1.text += "<b>For you:</b><br>";
        if (card.exhaustionHitPlayer1Failure > 0)
            failurePlayer1.text += "Exhaustion +" + card.exhaustionHitPlayer1Failure + "<br>";
        if (card.exhaustionHitPlayer1Failure < 0)
            failurePlayer1.text += "Exhaustion " + card.exhaustionHitPlayer1Failure + "<br>";
        if (card.frustrationHitPlayer1Failure > 0)
            failurePlayer1.text += "Frustration +" + card.frustrationHitPlayer1Failure + "<br>";
        if (card.frustrationHitPlayer1Failure < 0)
            failurePlayer1.text += "Frustration " + card.frustrationHitPlayer1Failure + "<br>";
        if (card.arousalHitPlayer1Failure > 0)
            failurePlayer1.text += "Arousal +" + card.arousalHitPlayer1Failure + "<br>";
        if (card.arousalHitPlayer1Failure < 0)
            failurePlayer1.text += "Arousal " + card.arousalHitPlayer1Failure + "<br>";
    }

    private void fillFailurePlayer2(Card card)
    {
        failurePlayer2.text = "";
        if (card.exhaustionHitPlayer2Failure != 0 || card.frustrationHitPlayer2Failure != 0 || card.arousalHitPlayer2Failure != 0)
            failurePlayer2.text += "<b>For her:</b><br>";
        if (card.exhaustionHitPlayer2Failure > 0)
            failurePlayer2.text += "Exhaustion +" + card.exhaustionHitPlayer2Failure + "<br>";
        if (card.exhaustionHitPlayer2Failure < 0)
            failurePlayer2.text += "Exhaustion " + card.exhaustionHitPlayer2Failure + "<br>";
        if (card.frustrationHitPlayer2Failure > 0)
            failurePlayer2.text += "Frustration +" + card.frustrationHitPlayer2Failure + "<br>";
        if (card.frustrationHitPlayer2Failure < 0)
            failurePlayer2.text += "Frustration " + card.frustrationHitPlayer2Failure + "<br>";
        if (card.arousalHitPlayer2Failure > 0)
            failurePlayer2.text += "Arousal +" + card.arousalHitPlayer2Failure + "<br>";
        if (card.arousalHitPlayer2Failure < 0)
            failurePlayer2.text += "Arousal " + card.arousalHitPlayer2Failure + "<br>";
    }

#endregion

    public void HideCardStats()
    {
        FadeOut();
        //body.text = "";
        //canvasGroup.alpha = 0f;
    }

    #region Position Tooltip

    /// <summary>
    /// Positions the tooltip within the screen
    /// </summary>
    private void Update()
    {
        Vector2 position = Input.mousePosition;

        float boundryX = Screen.width - (rectTransform.rect.width); //Anchored bottom left to match pointer.
        float boundryY = rectTransform.rect.height;

        /// Calculates the size of the tootip, (which needs a frame to set all things up) and then sets the pivots based on that.
        StartCoroutine(delayCalculateRectSize());

        if (position.x > boundryX + xOffset)
            pivotX = (position.x - boundryX) / rectTransform.rect.width;
        if (position.y < boundryY + yOffset)
            pivotY = (position.y - boundryY + rectTransform.rect.height) / rectTransform.rect.height;
        
        /// in case I want to anchor upwards.
        if (upperAnchor)
        {
            pivotY = 0;
            boundryY = Screen.height - rectTransform.rect.height;
            if (position.y > boundryY - yOffset)
                pivotY = (position.y - boundryY ) / rectTransform.rect.height;
        }

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

        pivotX = xOffset / rectTransform.rect.width;
        pivotY = (rectTransform.rect.height + yOffset) / rectTransform.rect.height;
    }

    #endregion

    // #TODO: Animate disolving.
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