using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueSystemMarkdownToEmphasis : MonoBehaviour
{
    #region parameters

    [Tooltip("Make sure it's the same Database the Dialogue Manager is uisng")]
    public DialogueDatabase dialogueDatabase;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ReplaceAllEntriesOfMarkdown();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ReplaceAllEntriesOfMarkdown()
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
    /// 
    /// </summary>
    public void AnalizeEntry(DialogueEntry currentEntry)
    {
        /*// check how many Scene-independant Events, (to see if there are more than 0).
        int onExecuteEventCount = currentEntry.onExecute.GetPersistentEventCount();

        /// maybe do a loop to check them all, in case there are more than one.
        var returnedPersistentTarget = (onExecuteEventCount > 0) ?  //"Kiss (Card)"
            currentEntry.onExecute.GetPersistentTarget(onExecuteEventCount - 1) : null;
        string persistentMethodName = (onExecuteEventCount > 0) ? //"ShowCard"
            currentEntry.onExecute.GetPersistentMethodName(onExecuteEventCount - 1) : null;
        */

        /// Replace *Sentence* to [em2]Sentence[/em2]
        string menuTextEm = currentEntry.MenuText;
        menuTextEm = ReplaceFirstOccurrence(menuTextEm, "*", "<i>");
        menuTextEm = ReplaceFirstOccurrence(menuTextEm, "*", "</i>");
        menuTextEm = ReplaceFirstOccurrence(menuTextEm, "*", "<i>");
        menuTextEm = ReplaceFirstOccurrence(menuTextEm, "*", "</i>");
        menuTextEm = ReplaceFirstOccurrence(menuTextEm, "*", "<i>");
        menuTextEm = ReplaceLastOccurrence(menuTextEm, "*", "</i>"); //this will close three pairs of * *. 
        currentEntry.MenuText = menuTextEm;


        string dialogueTextEm = currentEntry.DialogueText;
        dialogueTextEm = ReplaceFirstOccurrence(dialogueTextEm, "*", "<i>");
        dialogueTextEm = ReplaceFirstOccurrence(dialogueTextEm, "*", "</i>");
        dialogueTextEm = ReplaceFirstOccurrence(dialogueTextEm, "*", "<i>");
        dialogueTextEm = ReplaceFirstOccurrence(dialogueTextEm, "*", "</i>");
        dialogueTextEm = ReplaceFirstOccurrence(dialogueTextEm, "*", "<i>");
        dialogueTextEm = ReplaceLastOccurrence(dialogueTextEm, "*", "</i>");
        currentEntry.DialogueText = dialogueTextEm;

        /*
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
        */
    }

    public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.IndexOf(Find);
        if (Place>=0)
            return Source.Remove(Place, Find.Length).Insert(Place, Replace);
        return Source; //dont change anything,
    }

    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.LastIndexOf(Find);
        if (Place >= 0)
            return Source.Remove(Place, Find.Length).Insert(Place, Replace);
        return Source;
    }


}
