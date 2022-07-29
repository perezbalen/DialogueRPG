using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TalkToCombatManager : MonoBehaviour
{
    [Tooltip("Hook this to the script in the Combat Manager")]
    public TriggerEndEncounter endEncounterScript;

    /// <summary>
    /// This sends the OnConversationEnd to the Combat Manager, so it's deal with it there.
    /// It's used for the interruptions in the conversations dure to external factors (like getting tired).
    /// </summary>
    /// <param name="actor"></param>
    public void OnConversationEnd(Transform actor) // Assume this script is on the Dialogue Manager or player
    {
        /// This happense every time we change from one conversastion to antother, 
        /// and at the end. Meaning, every ConversatnonEnd.
       
        endEncounterScript.OnConversationEnd(actor);
    }
}
