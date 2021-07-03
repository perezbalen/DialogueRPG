using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class TooltipForDialogueSystem : MonoBehaviour
{
    //public TooltipTrigger tooltipTrigger;

    public CardDeck deckOfCards; //where the cards are stored. Do I need this?

    public CardInfo cardInfo;   //the tooltip thing to show the data
    
    //shown for debug.
    [SerializeField]
    private Card cardLoadedOnHover;

    [System.Serializable]
    private enum cardMethodNames  //Should be public?
    {
        ShowCardWin,
        ShowCardLose,
        //ShowCard, //This might get deprecated. 
        GetCardReady
    }

    /// <summary>
    /// gets the Card in the Dialogue Event that the entry will trigger, 
    /// and asks the corresponding Card for the data to display in the 
    /// CardInfo tooltip thing.
    /// </summary>
    public void OnHover()
    {
        //Gets if the player clicks on this button, the response is sent to the dialgue system
        Response response = GetComponent<StandardUIResponseButton>().response;

        //check how many Scene-independant Events, to see if there are more than 0.:
        int onExecuteEventCount = response.destinationEntry.onExecute.GetPersistentEventCount();

        //maybe do a loop to check them all, in case there are more than one.
        var returnedPersistentTarget = (onExecuteEventCount > 0) ?  //"Kiss (Card)"
            response.destinationEntry.onExecute.GetPersistentTarget(onExecuteEventCount - 1) : null;
        string persistentMethodName = (onExecuteEventCount > 0) ? //"ShowCard"
            response.destinationEntry.onExecute.GetPersistentMethodName(onExecuteEventCount - 1) : null;

        //If the Dialogue Entry has a Scene-Independant Event, and it is of a Card object
        if (returnedPersistentTarget != null)
        { 
            if (returnedPersistentTarget.GetType() == typeof(Card)) //Didnt use && because Null poiner exception.
            {
                //loads the card used in the Dialogue Entry
                cardLoadedOnHover = (Card)returnedPersistentTarget; 

                //Sets the script (No longer here. Moved to COmbat Manager).
                //SetScriptForCombat(response, persistentMethodName); 
            }
        }
        
        //Debug.Log("cardLoadedOnHover: " + cardLoadedOnHover);

        if (cardLoadedOnHover != null) //If the card excists.
        {
            cardLoadedOnHover.GetCardReady();
            cardInfo.hasCardData = cardLoadedOnHover.isCombat; //tells the cardInfo tooltip thing there's something to display.
        }
        else //No existe el objeto Card a activar
        {
            cardInfo.hasCardData = false;
            // Tell Tooltip to show nothing and hide.
            if (cardInfo != null) 
            { 
                cardInfo.HideCardStats(); 
            }
        }
    }

    /*
    /// <summary>
    /// Maybe esto no debería ser acá, pero no se me ocurre donde más,
    /// </summary>
    /// <param name="response"></param>
    /// <param name="methodName">The method name used in the entry. Like "ShowCard"</param>
    private void SetScriptForCombat(Response response, string methodName)
    {
        /// if the Event is a Card.GetCardReady, it means the dialogue entry is a quiestion node.
        /// so the script should be like
        /// Variable["LatestDiceRoll"] = RollDiceResult(Variable["EASY"] ); UpdateStat("", 0)
        string scriptForQuestionEntry = "Variable[\"LatestDiceRoll\"] = RollDiceResult(Variable[\"" +
            cardLoadedOnHover.cardDificulty.ToString().ToUpper() + "\"] ); UpdateStat(\"\", 0)";

        /// if the Event is a Card.ShowCard, it means the dialogue entry is a result.
        /// so the script should be untocuhed, and the Conditions should be like
        /// Variable["LatestDiceRoll"] >= Variable["EASY"]
        /// NOTE: ShowCard can be used for non-combat. So also check for Card.isCombat flag.
                string conditionForResponseEntry = "Variable[\"LatestDiceRoll\"] >= Variable[\"" +
            cardLoadedOnHover.cardDificulty.ToString().ToUpper() + "\"]";

        //if the card needs a roll
        if (cardLoadedOnHover.isCombat)
        {
            if (methodName == cardMethodNames.GetCardReady.ToString())
            {
                ///Get Card ready
                ///Modify the Script. (string composed a few lines before for easy reaing.
                response.destinationEntry.userScript = scriptForQuestionEntry;
                Debug.Log("Script: " + response.destinationEntry.userScript);
                Debug.Log("methodName:" + methodName);
            }
            if (methodName == cardMethodNames.ShowCardWin.ToString())
            {
                ///What to do if win card?
                ///Modify the Condition. (string composed a few lines before for easy reaing.
                response.destinationEntry.conditionsString = conditionForResponseEntry;
                Debug.Log("Conditions:" + response.destinationEntry.conditionsString);
                Debug.Log("methodName:" + methodName);
            }
            if (methodName == cardMethodNames.ShowCardLose.ToString())
            {
                ///What to do if loose card
                ///Modify the Condition. (string composed a few lines before for easy reaing.
                response.destinationEntry.conditionsString = conditionForResponseEntry;
                Debug.Log("Conditions:" + response.destinationEntry.conditionsString);
                Debug.Log("methodName:" + methodName);
            }
        }
        else
        {
            /// What o do if its not a combat card (isCombat==false)? 
            /// So far, I havent thought of a case where I need to inject scripts or conditions to those cards.
            /// Non combat cards dont roll, so they are allways the same (no roll to choos the side)
            /// So no script (question) or condition (to deremine answer) is needed.
        }
    }
    */

    public void OnUnhover()
    {
        ///Thios makes the botton forget the card before (since the dialogue sistem reuses response buttons
        /// not forgetting was making old tooltips show on new responses that had no card).
        cardLoadedOnHover = null;
        
        /*
        if (tooltipTrigger != null) 
            tooltipTrigger.content = string.Empty;
        */

        /* 
        //this deactivates the object refered in the "Activate On Hover" field of the dialgue option
        if (activateOnHover != null) 
            activateOnHover.SetActive(false);
        */

        //this deactivates the object refered in the "Activate On Hover" field of the dialgue option
        //if (loadCardOnHover != null)
        //    loadCardOnHover.SetActive(false); //I'm not activating and deactivating objects on hover.
    }

}
