using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class InitializeDialogeSystemCombat : MonoBehaviour
{
    #region parameters

    [Tooltip("Make sure it's the same Database the Dialogue Manager is uisng")]
    public DialogueDatabase dialogueDatabase;

    [System.Serializable]
    private enum cardMethodNames  //Should be public?
    {
        ShowCardWin,
        ShowCardLose,
        //ShowCard, //This might get deprecated. 
        GetCardReady
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetupCombatCoicesInDialogueSystem();
    }

    /// <summary>
    /// Based on the OnExecute Events, Thiis will override the Scripts and Conditions
    /// of the Dialogue Entryes, to make them behave like the card says.
    /// </summary>
    public void SetupCombatCoicesInDialogueSystem()
    {
        //Loop through all conversations
        foreach (Conversation conversation in DialogueManager.masterDatabase.conversations)
        {
            //Loop through all entries
            foreach (DialogueEntry entry in conversation.dialogueEntries)
            {
                //DialogueEntry entry = conversation.GetDialogueEntry("Kiss");
                AnalizeEntry(entry);
            }
        }
    }

    /// <summary>
    /// gets the Card in the Dialogue Event that the entry will trigger, 
    /// and asks the corresponding Card for the data to display in the 
    /// CardInfo tooltip thing.
    /// </summary>
    public void AnalizeEntry(DialogueEntry currentEntry)
    {
        /// check how many Scene-independant Events, (to see if there are more than 0).
        int onExecuteEventCount = currentEntry.onExecute.GetPersistentEventCount();

        /// maybe do a loop to check them all, in case there are more than one.
        var returnedPersistentTarget = (onExecuteEventCount > 0) ?  //"Kiss (Card)"
            currentEntry.onExecute.GetPersistentTarget(onExecuteEventCount - 1) : null;
        string persistentMethodName = (onExecuteEventCount > 0) ? //"ShowCard"
            currentEntry.onExecute.GetPersistentMethodName(onExecuteEventCount - 1) : null;

        /// If the Dialogue Entry has a Scene-Independant Event, and it is of a Card object
        if (returnedPersistentTarget != null)
        {
            if (returnedPersistentTarget.GetType() == typeof(Card)) //Didnt use && because Null poiner exception.
            {
                //loads the card used in the Dialogue Entry
                Card returnedCard = (Card)returnedPersistentTarget;

                //Sets the script (this is not working yet. Maybe should not do it here).
                SetScriptForEntry(currentEntry, returnedCard, persistentMethodName);
            }
        }
    }

    private void SetScriptForEntry(DialogueEntry currentEntry, Card currentCard, string methodName)
    {
        #region cosntruct the strings
        /// if the Event is a "Card.GetCardReady", it means the dialogue entry is a quiestion node.
        /// so the script should be like
        /// Variable["LatestDiceRoll"] = RollDiceResult(Variable["EASY"] ); UpdateStat("", 0)
        string scriptForQuestionEntry = "Variable[\"LatestDiceRoll\"] = RollDiceResult(Variable[\"" +
            currentCard.cardDificulty.ToString().ToUpper() + "\"] ); UpdateStat(\"\", 0)";
        /// if the Event is a "Card.ShowCard", it means the dialogue entry is a result.
        /// so the script should be untocuhed, and the Conditions should be like
        /// Variable["LatestDiceRoll"] >= Variable["EASY"]
        /// NOTE: ShowCard can be used for non-combat. So also check for Card.isCombat flag.
        string conditionForResponseEntryWin = "Variable[\"LatestDiceRoll\"] >= Variable[\"" +
            currentCard.cardDificulty.ToString().ToUpper() + "\"]";
        string conditionForResponseEntryLose = "Variable[\"LatestDiceRoll\"] < Variable[\"" +
            currentCard.cardDificulty.ToString().ToUpper() + "\"]";
        #endregion

        /// IDEA: Maybe here we could get the lines from the array of first lines, 
        /// but figure out when to call it. (Constantly?)
        
        /// if the card needs a roll
        if (currentCard.isCombat)
        {
            if (methodName == cardMethodNames.GetCardReady.ToString())
            {
                /// Get Card Ready
                /// Modify the Script. (string composed a few lines above for easy code reaing).
                currentEntry.userScript = scriptForQuestionEntry;
                ///Debug.Log("Script: " + currentEntry.userScript);
                ///Debug.Log("methodName:" + methodName);
            }
            if (methodName == cardMethodNames.ShowCardWin.ToString())
            {
                /// What to do if WIN card?
                /// Modify the Condition. (string composed a few lines above for easy code reaing).
                currentEntry.conditionsString = conditionForResponseEntryWin;
                ///Debug.Log("Conditions:" + currentEntry.conditionsString);
                ///Debug.Log("methodName:" + methodName);
            }
            if (methodName == cardMethodNames.ShowCardLose.ToString())
            {
                /// What to do if LOSE card
                /// Modify the Condition. (string composed a few lines above for easy code reaing).
                currentEntry.conditionsString = conditionForResponseEntryLose;
                ///Debug.Log("Conditions:" + currentEntry.conditionsString);
                ///Debug.Log("methodName:" + methodName);
            }
            /// remember that if the card is set to isCombat, and to ShowCardWin.
            /// And it's the first in a conversation, it will never trigger.
            /// Becasue it will set a conditon asking for previous roll that 
            /// did never happen. So it will imediately exit.
        }
        else
        {
            /// What o do if its not a combat card (isCombat==false)? 
            /// So far, I havent thought of a case where I need to inject scripts or conditions to those cards.
            /// Non combat cards dont roll, so they are allways the same (no roll to choos the side)
            /// So no script (question) or condition (to deremine answer) is needed.
        }
    }
}
