using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//I think this isn;t doing anything. 
//relpaced by TextResizeObserver
public class TooltipBody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameEvents.current.onTextSizeChanges += OnChangeTextSize;
    }

    private void OnEnable()
    {
        //The event is managed by class ChangeFontSize
        //The event variable is OnTextSizeChanged (from the class ChangeFontSize) 
        //UpdateTextSize is the local method that reacts to the message triggered buy the ChangeFontSize class.
        ChangeFontSize.OnTextSizeChanged += UpdateTextSize;
    }

    private void OnDisable()
    {
        ChangeFontSize.OnTextSizeChanged -= UpdateTextSize;
    }

    void UpdateTextSize(float newSize)
    {
        //maybe look for the silz in the variable currentsize in ChangeFontSize.
        //LeanTween.moveLocalY(gameObject, 1.6f, 1f).setEaseOutQuad();

        TextMeshProUGUI thisTMP = this.gameObject.GetComponent<TextMeshProUGUI>();
        //thisTMP.fontSize = thisTMP.fontSize += 5f;
        thisTMP.fontSize = newSize;
           
    }
}
