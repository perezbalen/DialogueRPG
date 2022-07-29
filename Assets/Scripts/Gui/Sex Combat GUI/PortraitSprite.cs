using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class PortraitSprite : MonoBehaviour
{
    public Image portrait;
    public DialogueActor dialogueActor;

    // Start is called before the first frame update
    void Start()
    {
        PlacePortrait();
    }

    // Update is called once per frame
    void OnValidate()
    {
        //PlacePortrait();
    }

    private void PlacePortrait()
    {
        portrait = gameObject.GetComponent<Image>();
        portrait.sprite = dialogueActor.spritePortrait;
    }
}
