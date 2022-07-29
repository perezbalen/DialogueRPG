using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class TriggerEndEncounter : MonoBehaviour
{
    //public DialogueSystemTrigger dialogueTrigger;

    private string previousConversationTitle = string.Empty;
    private int previousEntryID;

    private bool shouldPlayOnConversationEnd = false;

    // Event
    public delegate void ConversationEnded();
    public static event ConversationEnded OnConversationEnded;

    private void OnEnable()
    {
        //Event Observer
        PlayerSRPGSheet.OnHPPoolCritial += HPPoolCritial;
        PlayerSRPGSheet.OnHPPoolLimitReached += HPLimitReached;
    }
    private void OnDisable()
    {
        //Event Observer
        PlayerSRPGSheet.OnHPPoolCritial -= HPPoolCritial;
        PlayerSRPGSheet.OnHPPoolLimitReached -= HPLimitReached;
    }

    private bool resumeLastConversation = false;
    private bool shouldHideCardDisplay = false;

    /// Test queue
    Stack<ConversationSpot> conversationSpots = new Stack<ConversationSpot>();
    //Queue<ConversationSpot> conversationSpots = new Queue<ConversationSpot>();

    /// <summary>
    /// Jumps to the Critical conversation, and sets things up to return where we left off.
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="poolName"></param>
    public void HPPoolCritial (PlayerSRPGSheet sheet, PoolName poolName)
    {
        if (DialogueManager.isConversationActive)
        {
            //Constructs the new conversation name
            string characterName;
            if (sheet.name == "Player")
            {
                characterName = "Player ";
            }
            else
            {
                characterName = "Girl ";
            }
            string conversationTitle = characterName + poolName.ToString() + " Warning";

            //Saves the spot in the conversation
            previousConversationTitle = DialogueManager.lastConversationStarted;
            resumeLastConversation = true;

            /// for some reason this is getting null when we get two warnings in a row.
            /// THis prevents crash. Not sure if it actually fixes the ting.
            /// NOTE: the fact that previousEntryID does not update the second time might cause problems
            if (DialogueManager.currentConversationState != null)
            {
                previousEntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id;
            }

            /// Test Queue
            ConversationSpot conversationSpot = new ConversationSpot
            {
                ConversationTitle = DialogueManager.lastConversationStarted,
                CurrentActor = DialogueManager.currentActor,
                CurrentConversant = DialogueManager.currentConversant,
                EntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id
            };
            conversationSpots.Push(conversationSpot);

            Debug.Log($"Spot PUSHED: {conversationSpot.ConversationTitle}");

            //stops the combat (current) conversation, so the warning conversation can start.
            InterruptConversation();

            DialogueManager.StartConversation(conversationTitle, 
                DialogueManager.currentActor, DialogueManager.currentConversant); //after this is done OnConversationEnd is called.
        }
    }

    public void HPLimitReached (PlayerSRPGSheet sheet, PoolName poolName)
    {
        //Constructs the new conversation name
        string characterName;
        if (sheet.name == "Player")
        {
            characterName = "Player ";
        }
        else
        {
            characterName = "Girl ";
        }
        string conversationTitle = characterName + poolName.ToString() + " Full";

        /// there was not previous conversation saving here.
        /// Saves the spot in the conversation
        previousConversationTitle = DialogueManager.lastConversationStarted;
        resumeLastConversation = true;

        /// Test Queue
        ConversationSpot conversationSpot = new ConversationSpot
        {
            ConversationTitle = DialogueManager.lastConversationStarted,
            CurrentActor = DialogueManager.currentActor,
            CurrentConversant = DialogueManager.currentConversant,
            EntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id
        };
        conversationSpots.Push(conversationSpot);

        Debug.Log($"Spot PUSHED: {conversationSpot.ConversationTitle}");

        InterruptConversation();

        /*
        if (poolName.ToString() == PoolName.Exhaustion.ToString() ||
            poolName.ToString() == PoolName.Frustration.ToString())
        {
            shouldHideCardDisplay = true;
        }
        */

        DialogueManager.StartConversation(conversationTitle,
            DialogueManager.currentActor, DialogueManager.currentConversant);


    }

    /// <summary>
    /// Stops the DialogueManager conversation, but setting a flag that prevents the 
    /// OnConversationEnd to execute code intended for an actual stopping, not an
    /// Interruption, like we want.
    /// </summary>
    private void InterruptConversation()
    {
        shouldPlayOnConversationEnd = false;
        DialogueManager.StopConversation();
        shouldPlayOnConversationEnd = true;
    }

    /// <summary>
    /// This script should be on the Dialogue Manager or player
    /// Since it's not, there's a OnConversationEnd on the Dialogue Manager 
    /// On the script TalkToCombatManager, that call this.
    /// </summary>
    /// <param name="actor"></param>
    public void OnConversationEnd(Transform actor) 
    {
        if (shouldPlayOnConversationEnd)
        {
            //test Queue
            ConversationSpot conversationSpot = conversationSpots.Pop();
            
            Debug.Log($"Spot popped: {conversationSpot.ConversationTitle}");

            //nedd a way to invert the dequeue.

            //if there is a last conversation saved and we marked to resume it
            if ( conversationSpot!=null )
            {
                DialogueManager.StartConversation(conversationSpot.ConversationTitle,
                    conversationSpot.CurrentActor,
                    conversationSpot.CurrentConversant,
                    conversationSpot.EntryID);

                //Cleans previousConversationTitle so we don't enter this if indefenitely
                //previousConversationTitle = string.Empty;
                //resumeLastConversation = false;
            }

            /*
            //if there is a last conversation saved and we marked to resume it
            if (!string.IsNullOrEmpty(previousConversationTitle) && resumeLastConversation)
            {
                DialogueManager.StartConversation(previousConversationTitle,
                    DialogueManager.currentActor, DialogueManager.currentConversant, previousEntryID);

                //Cleans previousConversationTitle so we don't enter this if indefenitely
                previousConversationTitle = string.Empty;
                resumeLastConversation = false;
            }
            */
        }

        /// Tells whoever is subscribed that the conversation has ended
        /// Intended to hide the CardDisplayMove
        ///  Note: This might not work as I expect. Feels like a patch.
        ///  
        /*
        if (shouldHideCardDisplay)
        {
            OnConversationEnded?.Invoke(); // HideCardDisplay()
            shouldHideCardDisplay = false; //reset the switch for the next time.
        }
        */
    }
}
