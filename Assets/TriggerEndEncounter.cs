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

            Debug.Log("Gets here: [" + previousConversationTitle + "] Id: [" + previousEntryID);
            Debug.Log("DialogueManager.currentConversationState.subtitle.dialogueEntry: " 
                + DialogueManager.currentConversationState.subtitle.dialogueEntry);
            previousEntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id;
            Debug.Log("But not here: [" + previousConversationTitle + "] Id: [" + previousEntryID);

            //stops the conversation
            DialogueManager.StopConversation();
            
            Debug.Log("StopConversation: [" + previousConversationTitle + "] Id: [" + previousEntryID);
                       
            resumeLastConversation = true;
            DialogueManager.StartConversation(conversationTitle, 
                DialogueManager.currentActor, DialogueManager.currentConversant);

            Debug.Log("HPPoolCritial: " + conversationTitle);

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
        Debug.Log("OnConversationEnd: [" + previousConversationTitle + "] will resume? " + resumeLastConversation);

        //OnConversationEnd: [] will resume? True 

        if (!string.IsNullOrEmpty(previousConversationTitle) && resumeLastConversation)
        {
            Debug.Log("Inside the if: " + previousConversationTitle + " will resume? " + resumeLastConversation);

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

        /*
        //Saves the spot in the conversation
        previousConversationTitle = DialogueManager.lastConversationStarted;
        previousEntryID = DialogueManager.currentConversationState.subtitle.dialogueEntry.id;
        resumeLastConversation = true; // for debug. THis makes the conversation enter in the if loop of the OnConversatioEnd.
        */
        DialogueManager.StopConversation();

        //Debug.Log("HPLimitReached and stopped. Ready to start limit conversation: [" + conversationTitle +"]");
        DialogueManager.StartConversation(conversationTitle,
            DialogueManager.currentActor, DialogueManager.currentConversant);

        Debug.Log("HPLimitReached: ["+ conversationTitle + "] " + resumeLastConversation);
    }



    /*
    public void ExhaustionLimit(PlayerSRPGSheet sheet) 
    {
        if (sheet.name == "Player")
        {
            dialogueTrigger.conversation = "Player Exhausted Full";
        }
        else
        {
            dialogueTrigger.conversation = "Girl Exhausted Full";
        }
        dialogueTrigger.replace = true;
        dialogueTrigger.OnUse();
    }
    public void FrustrationLimit(PlayerSRPGSheet sheet) {
        if (sheet.name == "Player")
        {
            dialogueTrigger.conversation = "Player Frustrated Full";
        }
        else
        {
            dialogueTrigger.conversation = "Girl Frustrated Full";
        }
        dialogueTrigger.replace = true;
        dialogueTrigger.OnUse();
    }
    public void ArousalnLimit(PlayerSRPGSheet sheet) {
        Debug.Log(sheet.name + "'s ArousalnLimit.");
        if (sheet.name == "Player")
        {
            dialogueTrigger.conversation = "Player Aroused Full";
        }
        else
        {
            dialogueTrigger.conversation = "Girl Aroused Full";
        }
        dialogueTrigger.replace = true;
        dialogueTrigger.OnUse();
    }

    public void ExhaustionCritial(PlayerSRPGSheet sheet) {
        if (DialogueManager.isConversationActive)
        {
            ConversationPositionStack.PushConversationPosition();

            string conversation; 

            if (sheet.name == "Player")
            {
                conversation = "Player Exhausted Warning";
                //dialogueTrigger.conversation = "Player Exhausted Warning";
            }
            else
            {
                conversation = "Girl Exhausted Warning";
                //dialogueTrigger.conversation = "Girl Exhausted Warning";
            }

            Debug.Log(sheet.name + "'s ExhaustionCritial.");
            dialogueTrigger.replace = false;
            dialogueTrigger.OnUse();
        }
    }
    public void FrustrationCritial(PlayerSRPGSheet sheet) {
        if (sheet.name == "Player")
        {
            dialogueTrigger.conversation = "Player Frustrated Warning";
        }
        else
        {
            dialogueTrigger.conversation = "Girl Frustrated Warning";
        }
        Debug.Log(sheet.name + "'s FrustrationCritial.");
        dialogueTrigger.replace = false;
        dialogueTrigger.OnUse();
    }
    public void ArousalCritial(PlayerSRPGSheet sheet) {
        if (sheet.name == "Player")
        {
            dialogueTrigger.conversation = "Player Aroused Warning";
        }
        else
        {
            dialogueTrigger.conversation = "Girl Aroused Warning";
        }
        Debug.Log(sheet.name + "'s ArousalCritial.");
        dialogueTrigger.replace = false;
        dialogueTrigger.OnUse();
    }
    */


}
