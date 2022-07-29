using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I think I don't need this
/// </summary>
public class CardObject : MonoBehaviour
{
    #region Parameters

    [Header("4. UNTIL I FIGURE OUT A BETTER WAY TO DO THIS.")]
    [Header("3. THIS ARE HERE SO THE DIALOGUE SYSTEM CAN SEE THEM,")]
    [Header("2. YOU NEED TO DO THAT ON THE SCRIPTABLE OBJECT.")]
    [Header("1. YOU CAN'T CHANGE ANY OF THIS VALUES HERE.")]

    [Tooltip("Plug the Card Display to Display this images. (now done automatically)")]
    [SerializeField]
    public CardDisplay cardDisplay;

    [Header("The Scriptable Object with the particular Card.")]
    [SerializeField]
    private Card card;

    [Header("At this point, you cant modify this. Only for show and debug.")]
    [Tooltip("The data comes from the scriptable object. Maybe I shoud only use this?")]
    public string cardName;
    [Multiline]
    public string internalDescription ;

    [System.Serializable]
    public enum SexActType
    {
        Kissing,
        Breastsplay,
        Undressing,
        Blowjob,
        Cunnilingus,
        Anal,
        Handjob,
        Tittyfuck,
        Feetjob
    };
    public SexActType sexActType;

    /// <summary>
    /// How to find them easy
    /// </summary>
    /*
    [System.Serializable]
    public enum OrderOfPlay
    {
        ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE
    };
    public OrderOfPlay orderOfPlay;
    */

    [System.Serializable]
    public enum StatDependency { Stamina, Technique, Charisma, Will };
    public StatDependency statDependency;

    [System.Serializable]
    public enum CardDificulty { Easy, Medium, Hard }
    public CardDificulty cardDificulty;

    [Space]

    [Header("Success")]
    public Sprite sucessImage;
    [TextArea]
    public string successText = "I think this will also be here for reference. The actual text goes in the dialogue system";

    [Header("Success Player 1")]
    [Tooltip("What does this card does tp player 1 HPs when the roll wins.")]
    public int exhaustionHitPlayer1Success = 0;
    public int frustrationHitPlayer1Success = 0;
    public int arousalHitPlayer1Success = 0;

    [Header("Success Player 2")]
    [Tooltip("What does this card does tp player 2 HPs when the roll fails.")]
    public int exhaustionHitPlayer2Success = 0;
    public int frustrationHitPlayer2Success = 0;
    public int arousalHitPlayer2Success = 0;

    [Space]

    [Header("Failure")]
    public Sprite failImage;
    [TextArea]
    public string failText = "I think this will also be here for reference. The actual text goes in the dialogue system";

    [Header("Failure Player 1")]
    [Tooltip("What does this card does tp player 1 HPs when the roll wins.")]
    public int exhaustionHitPlayer1Failure = 0;
    public int frustrationHitPlayer1Failure = 0;
    public int arousalHitPlayer1Failure = 0;

    [Header("Failure Player 2")]
    [Tooltip("What does this card does tp player 2 HPs when the roll fails.")]
    public int exhaustionHitPlayer2Failure = 0;
    public int frustrationHitPlayer2Failure = 0;
    public int arousalHitPlayer2Failure = 0;

    #endregion

    #region Constructors & Mantainance

    // Start is called before the first frame update
    void Start()
    {
        CopySOtoThis();
    }
    private void OnValidate()
    {
        CopySOtoThis();
    }

    /// <summary>
    /// Copies the Scriptable Object to the
    /// MonoBehaviour Object
    /// </summary>
    private void CopySOtoThis()
    {
        cardDisplay = gameObject.GetComponentInParent<CardDisplay>();

        cardName = card.name;
        internalDescription = card.internalDescription;

        sexActType = (SexActType)card.sexActType;
        //orderOfPlay = (OrderOfPlay)card.orderOfPlay;
        statDependency = (StatDependency)card.statDependency;
        cardDificulty = (CardDificulty)card.cardDificulty;
        
        sucessImage = card.sucessImage;
        successText = card.successText;

        exhaustionHitPlayer1Success = card.exhaustionHitPlayer1Success;
        frustrationHitPlayer1Success = card.frustrationHitPlayer1Success;
        arousalHitPlayer1Success = card.arousalHitPlayer1Success;

        exhaustionHitPlayer2Success = card.exhaustionHitPlayer2Success;
        frustrationHitPlayer2Success = card.frustrationHitPlayer2Success;
        arousalHitPlayer2Success = card.arousalHitPlayer2Success;

        failImage = card.failImage;
        failText = card.failText;

        exhaustionHitPlayer1Failure = card.exhaustionHitPlayer1Failure;
        frustrationHitPlayer1Failure = card.frustrationHitPlayer1Failure;
        arousalHitPlayer1Failure = card.arousalHitPlayer1Failure;

        exhaustionHitPlayer2Failure = card.exhaustionHitPlayer2Failure;
        frustrationHitPlayer2Failure = card.frustrationHitPlayer2Failure;
        arousalHitPlayer2Failure = card.arousalHitPlayer2Failure;
    }

    #endregion
   
    /// <summary>
    /// Gets the card on the Stage, ready to get info out, 
    /// but DON'T show it on stage yet.
    /// </summary>
    public void GetCardReady()
    {
        cardDisplay.SetButDontShow(this.card);
        //Shout message: look at me, I 

        //show the tooltip. cardobject shows itself.

        //maybe ad a show current card (already on the CardDisplay) for the results in the carddissplay so it's the same every time. 
    }

    /// <summary>
    /// Makes this object the one shown on stage
    /// </summary>
    /// <param name="rollSucceded">True if it should display win reult</param>
    public void ShowCard(bool rollSucceded)
    {
        cardDisplay.SetCard(this.card, rollSucceded);
    }

}
