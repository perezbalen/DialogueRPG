using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Add this to any text that should resize when the game text sixe changes. (font size)
/// 
/// The observer pattern is a software design pattern in which an **object**, called the **subject**, 
/// maintains a list of its **dependents**, called **observers**, 
/// and notifies them automatically of any state changes, usually by calling one of their methods.
/// </summary>
public class TextResizeObserver : MonoBehaviour
{
    [Tooltip("Text wil change to 22.7 in Medium. How much should this be bigger (or smaller) than that?")]
    public float embigenFactor;


    private void OnEnable() 
    {
        //The event is managed by class ChangeFontSize
        //The event variable is OnTextSizeChanged (from the class ChangeFontSize) 
        //UpdateTextSize is the local method that reacts to the message triggered buy the ChangeFontSize class.
        ChangeFontSize.OnTextSizeChanged += UpdateTextSize;
        
        /// When the object is enambled, it must find out the current fotn size
        /// and adjust itself to the norm.
        /// 22.7
        
        UpdateTextSize(ChangeFontSize.currentSize);
    }

    private void  OnDisable()
    {
        ChangeFontSize.OnTextSizeChanged -= UpdateTextSize;
    }

    /// <summary>
    /// The code that gets executed when the event OnTextSizeChanged is called (the "alarm" is heard)
    /// </summary>
    /// <param name="newSize">New font side to change the text to. Note that this takes the local embigen Factor into account.</param>
    void UpdateTextSize(float newSize)
    {
        TextMeshProUGUI thisTMP = this.gameObject.GetComponent<TextMeshProUGUI>();
        if (thisTMP != null)
        {
            thisTMP.fontSize = newSize + embigenFactor;
        }
    }

}
