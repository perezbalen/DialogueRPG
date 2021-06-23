using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;


public class TriggerEndEncounter : MonoBehaviour
{
    //public DialogueSystemTrigger dialogueTrigger;

    private string previousConversationTitle = string.Empty;
    private int previousEntryID;

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

    /// <summary>
    /// Jumps to the Critical conversation, and sets things up to return where we left off.
    /// </summary>
    /// <param name="sheet"></param>
    /// <param name="poolName"></param>
    public void HPPoolCritial (PlayerSRPGSheet sheet, PlayerSRPGSheet.PoolNames poolName)
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

            Debug.Log("previousConversationTitle? [" + previousConversationTitle + "]");
            Debug.Log("currentConversationState? [" + 
                DialogueManager.currentConversationState + "]");

            /// for some reason this is getting null when we get two warnings in a row.
            /// THis prevents crash. Not sure if it actually fixes the ting.
            /// NOTE: the fact that previousEntryID does not update the second time might cause problems
            if (DialogueManager.currentConversationState!=null) 
                previousEntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id;
            
            //stops the conversation
            DialogueManager.StopConversation();

            /*
            Debug.Log("previousConversationTitle: [" + previousConversationTitle + "] previousEntryID: [" + previousEntryID);
            Debug.Log("conversationTitle: [" + conversationTitle);
            Debug.Log("resumeLastConversation: [" + resumeLastConversation);
            */

            resumeLastConversation = true;
            DialogueManager.StartConversation(conversationTitle, 
                DialogueManager.currentActor, DialogueManager.currentConversant);
        }
    }

    /// <summary>
    /// This script should be on the Dialogue Manager or player
    /// Since it's not, there's a OnConversationEnd on the Dialogue Manager 
    /// On the script TalkToCombatManager, that call this.
    /// </summary>
    /// <param name="actor"></param>
    public void OnConversationEnd(Transform actor) 
    {
        if (!string.IsNullOrEmpty(previousConversationTitle) && resumeLastConversation)
        {
            DialogueManager.StartConversation(previousConversationTitle, 
                DialogueManager.currentActor, DialogueManager.currentConversant, previousEntryID);

            //Cleans previousConversationTitle so we don't enter this if indefenitely
            previousConversationTitle = string.Empty;
            resumeLastConversation = false;            
        }
    }

    public void HPLimitReached (PlayerSRPGSheet sheet, PlayerSRPGSheet.PoolNames poolName)
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
   
        DialogueManager.StopConversation();
        DialogueManager.StartConversation(conversationTitle,
            DialogueManager.currentActor, DialogueManager.currentConversant);
    }

}
