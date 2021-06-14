using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Think of this as the hand of the DM, where he has the card. Or the table, where the card is displayed on.
/// </summary>
[ExecuteInEditMode]
public class CardDisplay : MonoBehaviour
{
    [Tooltip("Put here the Card scriptable object that corresponds to a sex act card")]
    //[Header("The order matters. Or I think it will.")]
    //[SerializeField]
    //private Card card; //dpnt know why I need this?

    [Header("Debug stuff")]
    public bool debugIsSuccess;

    public Card currentCard; 
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //currentCard = cards[0];
        //currentCard = card;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Card.OnAskedToGetReady += SetButDontShow;
        Card.OnAskedToShowCard += SetCard;
    }

    private void OnDisable()
    {
        Card.OnAskedToGetReady -= SetButDontShow;
        Card.OnAskedToShowCard -= SetCard;
    }

    /// <summary>
    /// Activates the Card that we want playing.
    /// PD: Check we don't draw it before we want.
    /// </summary>
    /// <param name="possission"></param>
    public void SetActiveCard(int possission)
    {
        //maybe not needed.
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
        //Debug.Log("isItSuccess:" + rollSucceed);
        if (rollSucceed)
        {
            spriteRenderer.sprite = currentCard.sucessImage;
        }
        else
        {
            spriteRenderer.sprite = currentCard.failImage;
        }
    }
}
