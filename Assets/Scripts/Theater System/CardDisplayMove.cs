using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class CardDisplayMove : MonoBehaviour
{

    public Card currentCard;
    //[Header("Debug. Turn private,")]
    private Image image;
    private MMFeedbacks mmFeedbacks;


    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        mmFeedbacks = gameObject.GetComponent<MMFeedbacks>();
        ShowCardDisplay(false); //Hides the CardDisplayUI on start. This is assuming we want to start with it off.
    }

    private void OnEnable()
    {
        Card.OnAskedToGetReady += SetButDontShow;
        Card.OnAskedToShowCard += SetCard;
        TriggerEndEncounter.OnConversationEnded += HideCardDisplay; //might not need it. Chech in TriggerEndEncounter OnConversationEnd
    }

    private void OnDisable()
    {
        Card.OnAskedToGetReady -= SetButDontShow;
        Card.OnAskedToShowCard -= SetCard;
        TriggerEndEncounter.OnConversationEnded -= HideCardDisplay; //might not need it. 
    }

    /// <summary>
    /// Loads a Card into the CardDisplay. 
    /// But doesn't update the image yet.
    /// It waits for the ShowSettedCard command, which decides which image to show
    /// This is so the Question node has access to info, and can show the player the options
    /// But it still has to wait for a roll to know what to show 
    /// (and the Answer dialogue node is the one that actually shows the card, 
    /// with the ShowSettedCard event).
    /// </summary>
    /// <param name="newCard"></param>
    public void SetButDontShow(Card newCard)
    {
        currentCard = newCard;
    }

    /// <summary>
    /// Takes an Card Scriptable Object and replaces it with the current.
    /// To be used with all the CardObject
    /// </summary>
    /// <param name="newCard">The card to show on stage</param>
    /// <param name="rollSucceeded">SHould we show Success of Failure side of the card</param>
    public void SetCard(Card newCard, bool rollSucceeded)
    {
        currentCard = newCard;
        ShowSuccessCardImage(rollSucceeded); //do I need ShowSuccess public?
    }

    /// <summary>
    /// Shows the current setted card. It shows the image in the theater.
    /// </summary>
    /// <param name="rollSucceeded"></param>
    public void ShowSettedCard(bool rollSucceeded)
    {
        ShowSuccessCardImage(rollSucceeded);
    }

    /// <summary>
    /// Shows the Successful picture (if true)
    /// ot the Failure picture (if false)
    /// </summary>
    /// <param name="rollSucceed">does the roll succeed?</param>
    public void ShowSuccessCardImage(bool rollSucceed)
    {
        ShowCardDisplay(true);
        mmFeedbacks.PlayFeedbacks();
        if (rollSucceed)
        {
            image.sprite = currentCard.sucessImage;
        }
        else
        {
            image.sprite = currentCard.failImage;
        }
    }

    /// <summary>
    /// When off, sets the non-image (a whaite square) to transaprent
    /// When on, sets the aplha to 1, so the sprite is shown 
    /// (the color should be white to get no tint)
    /// </summary>
    /// <param name="show"></param>
    public void ShowCardDisplay(bool show)
    {
        Color tempColor = image.color;
        tempColor.a = show ? 1f : 0f;
        image.color = tempColor;
    }

    /// <summary>
    /// A convinient way to hide the CardDisplay.
    /// It does the same as ShowCardDisplay(false);
    /// </summary>
    public void HideCardDisplay()
    {
        ShowCardDisplay(false);
    }

}
