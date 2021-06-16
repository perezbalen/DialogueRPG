using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(DialogueSystemTrigger))]
public class TriggerDeathTest : MonoBehaviour
{
    public Conversation conversation;
    public DialogueEntry entry;

    public DialogueSystemTrigger dialogueTrigger;
    // Start is called before the first frame update
    private void Start()
    {
        dialogueTrigger = GetComponent<DialogueSystemTrigger>();
    }

    public void OnButtonClicked()
    {
        //conversation = DialogueManager.masterDatabase.GetConversation("Act BJ");
        
        //dialogueTrigger.replace = true; //replaces the other conversation.
        dialogueTrigger.conversation = "Girl Exhausted Full";
        /*
        dialogueTrigger.conversation = "Girl Exhausted Warning";
        dialogueTrigger.conversation = "Girl Frustrated Full";
        dialogueTrigger.conversation = "Girl Frustrated Warning";
        dialogueTrigger.conversation = "Girl Aroused Full";
        dialogueTrigger.conversation = "Girl Aroused Warning";
        dialogueTrigger.conversation = "Player Exhausted Full";
        dialogueTrigger.conversation = "Player Exhausted Warning";
        dialogueTrigger.conversation = "Player Frustrated Full";
        dialogueTrigger.conversation = "Player Frustrated Warning";
        dialogueTrigger.conversation = "Player Aroused Full";
        dialogueTrigger.conversation = "Player Aroused Warning";
        */

        dialogueTrigger.OnUse();
        //Debug.Log(PixelCrushers.DialogueSystem.DialogueManager.lastConversationStarted);
    }

}
