using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TalkToDialgueManager : MonoBehaviour
{
    public Conversation conversation;
    public DialogueEntry entry;
    public void OnButtonClicked()
    {
        conversation = DialogueManager.masterDatabase.GetConversation("Act BJ");
        entry = conversation.GetDialogueEntry("Kiss");
        //Field.SetValue(entry.fields, "Activate on Hover", SomeGameObject.name);
        //Debug.Log("All Ok.");

        //creo que ya no necesito el tag. "Load on Hover. Lo puedo sacar del onExecute?"

        //Esto no queda salvado. Hay que hacerlo cada vez.

        //lo que escoje el player
        entry.MenuText = "[em3]I added this part using the api.[/em3]";
        //Debug.Log("entry.MenuText:" + entry.MenuText);
       
        //lo que responde el juego
        entry.DialogueText = "This was Kiss it. Now it's [em2] suck it.[/em2].";
        //Debug.Log("entry.DialogueText:" + entry.DialogueText);

        //Adds/changes the Field.
        Field.SetValue(entry.fields, "Load Card On Hover", "Card Name Test");

        //entry.userScript = "Variable[\"LatestDiceRoll\"] = RollDiceResult(Variable[\"HARD\"]";
        
        //gets "getCardReady"/"ShowCard"
        Debug.Log("GetPersistentMethodName: " + entry.onExecute.GetPersistentMethodName(0) );

        //gets "Kiss (Card)"
        Debug.Log("GetPersistentTarget: " + entry.onExecute.GetPersistentTarget(0));

        //gets 1 (in case of a loop to check all)
        Debug.Log("GetPersistentEventCount: " + entry.onExecute.GetPersistentEventCount());

        //gets true == true (or wahtever conditon)
        Debug.Log("conditionsString: " + entry.conditionsString);


        //string methodName = entry.onExecute.GetPersistentMethodName(0); //"getCardReady"

        //gets the card on the onExecute[0] and puts the success text instead of the one on the DiagSys
        Card target = (Card)entry.onExecute.GetPersistentTarget(0); //Kiss (card)
        entry.DialogueText = target.successText;

        //Debug.Log("GetPersistentTarget:" + entry.onExecute.SetPersistentListenerState(0));
    }

}
