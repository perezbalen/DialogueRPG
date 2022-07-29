using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Card", menuName ="Card")]
public class Card : ScriptableObject
{

    #region Parameters
    public new string name; //the name is the file/object name

    [Tooltip("Flag to tell if Roll stats should be shown. (And wantever else I manage to do)")]
    [Header("If the card is set to isCombat, to ShowCardWin, and it's the first in a conversation, it will never trigger.")]
    public bool isCombat = true;
    [Space]

    [Multiline]
    public string internalDescription = "This wnot show in game. It's as a helper for design. Actual description will go in dialogue system.";

    [System.Serializable]
    public enum SexActType { 
        Face, 
        Tits, 
        Pussy, 
        Ass, 
        Eat, 
        CumMan, 
        CumGirl, 
        Foreplay, 
        Pee,
        Dildo,
        Fist
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
    public enum CardDificulty { Easy, Medium, Hard}
    public CardDificulty cardDificulty;
    
    [Space]
  
    [Header("Success")]
    public Sprite sucessImage;
    [TextArea]
    public string successText = "I think this will also be here for reference. The actual text goes in the dialogue system";

    public List<string> firstLinesSuccess; //not implemented yet.

    [Header("Success Player 1")]
    [Tooltip("What does this card does tp player 1 HPs when the roll wins.")]
    public int exhaustionHitPlayer1Success = 1; 
    public int frustrationHitPlayer1Success = 0;
    public int arousalHitPlayer1Success = 0;

    [Header("Success Player 2")]
    [Tooltip("What does this card does tp player 2 HPs when the roll fails.")]
    public int exhaustionHitPlayer2Success = 1;
    public int frustrationHitPlayer2Success = 0;
    public int arousalHitPlayer2Success = 0;
    
    [Space]
    
    [Header("Failure")]
    public Sprite failImage;
    [TextArea]
    public string failText = "I think this will also be here for reference. The actual text goes in the dialogue system";

    public List<string> firstLinesFailure; //not implemented yet.

    [Header("Failure Player 1")]
    [Tooltip("What does this card does tp player 1 HPs when the roll wins.")]
    public int exhaustionHitPlayer1Failure = 1;
    public int frustrationHitPlayer1Failure = 0;
    public int arousalHitPlayer1Failure = 0;

    [Header("Failure Player 2")]
    [Tooltip("What does this card does tp player 2 HPs when the roll fails.")]
    public int exhaustionHitPlayer2Failure = 1;
    public int frustrationHitPlayer2Failure = 1;
    public int arousalHitPlayer2Failure = 0;

    #endregion 


    public delegate void AskedToGetReady(Card card);
    public static event AskedToGetReady OnAskedToGetReady; 
    //CardDisplay should subscribe to this.

    public delegate void AskedToShowCard(Card card, bool rollSucceded);
    public static event AskedToShowCard OnAskedToShowCard;

    /// <summary>
    /// The dialogue system calls this.
    /// </summary>
    public void GetCardReady()
    {
        //cardDisplay.SetButDontShow(this.card);
        //Shout message: look at me, I 
        OnAskedToGetReady?.Invoke(this); //this invokes the event.
    }

    /// <summary>
    /// Makes this object the one shown on stage
    /// </summary>
    /// <param name="rollSucceded">True if it should display win reult</param>
    public void ShowCard(bool rollSucceded)
    {
        //cardDisplay.SetCard(this.card, rollSucceded);
        OnAskedToShowCard?.Invoke(this, rollSucceded);
    }

    /// <summary>
    /// The same as show card, (Makes this object the one shown on stage),
    /// but has no arguments. Shows the Success State.
    /// A hack for the Dialogue System.
    /// </summary>
    public void ShowCardWin()
    {
        ShowCard(true);
    }

    /// <summary>
    /// The same as show card, (Makes this object the one shown on stage),
    /// but has no arguments. Shows the Failure State.
    /// </summary>
    public void ShowCardLose()
    {
        ShowCard(false);
    }

    /// <summary>
    /// The same as ShowCardWin, but doesn't get Conditions and Scripts Overwritten on initialization
    /// </summary>
    public void ShowNonCombatCardWinSide()
    {
        ShowCard(true);
    }

    /// <summary>
    /// The same as ShowCardLoose, but doesn't get Conditions and Scripts Overwritten on initialization
    /// </summary>
    public void ShowNonCombatCardLoseSide()
    {
        ShowCard(false);
    }
}
