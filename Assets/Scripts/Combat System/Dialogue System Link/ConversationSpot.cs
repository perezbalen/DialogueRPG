using System;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// A spot in a conversation so to easily return to it.
/// </summary>
public class ConversationSpot 
{
    /// DialogueManager.StartConversation( previousConversationTitle,
    ///      DialogueManager.currentActor, DialogueManager.currentConversant,
    ///      previousEntryID);

    public string ConversationTitle { set; get; }
    public Transform CurrentActor { set; get; }
    public Transform CurrentConversant { set; get; }
    public int EntryID { set; get; }
}
