using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Accesability option to make the game's text bigger or smaller.
/// Texts that want to be affected, 
/// have to add the script TextResizerObserver to them
/// </summary>
public class ChangeFontSize : MonoBehaviour
{
    #region Parameters
    [Header("Font sizes")]
    public float sizeXL;
    public float sizeL;
    public float sizeM;
    public float sizeS;
    public float sizeXS;

    //public TextMeshProUGUI textField;
    //public List<TextMeshProUGUI> textFields; //lo quité cuando pasé a delegates
    enum TextSizeCategory {XS,S,M,L,XL};
    [Header("Current Text Size")]
    [SerializeField] private TextSizeCategory currentTextSizeCategory = TextSizeCategory.M;
    /// <summary>
    /// currentSize is static, so it's a class variable and not an object varaible. 
    /// So I can use it by the TextResizerOvserver that doesn't have access to any ChangeFontSize objects.
    /// I don't know, but I think there are no ChangeFontSize objects. But if they were, 
    /// The currentSize should be the same across the entire game. (for variations, the TextResizeObserver has a modifier)
    /// </summary>
    public static float currentSize; 
    #endregion

    #region Delegates
    //delegates
    public delegate void TextSizeChanged(float size);       //no parameters. Return void.
    public static event  TextSizeChanged OnTextSizeChanged; //event vaiable.
                                                            //public, so can be called from outsede.
                                                            //static, so no need for instance of object this clase. 
                                                            //of type of *delagate* created above.
                                                            //Down, this class should call the event when appropiate.
    #endregion

    /// <summary>
    /// Moves to the next TextSizeCategory (S,M,L...)
    /// then calls the function to turn that category into a number
    /// </summary>
    public void IncreaseSize()
    {
        var length = System.Enum.GetNames(typeof(TextSizeCategory)).Length;
        currentTextSizeCategory = (TextSizeCategory)(
            ((int)currentTextSizeCategory + 1) //% length  //the "% length" lets cycle
            );
        //prevents cicling
        if ((int)currentTextSizeCategory == length)
        {
            currentTextSizeCategory = (TextSizeCategory)(
            (int)currentTextSizeCategory - 1); //stays there.
        }
        //end prevents cicling
        FindCurrentSize();
    }

    /// <summary>
    /// Moves to the previous TextSizeCategory (S,M,L...)
    /// then calls the function to turn that category into a number
    /// </summary>
    public void DecreaseSize()
    {
        //this chunk sets the currentTextSizeCategory (L,M,S,etc) 
        var length = System.Enum.GetNames(typeof(TextSizeCategory)).Length;
        currentTextSizeCategory = (TextSizeCategory)(
            ((int)currentTextSizeCategory - 1) 
            );
        if ((int)currentTextSizeCategory < 0)
        {
            //currentTextSizeCategory = (TextSizeCategory)length - 1; // cicles
            currentTextSizeCategory = 0; //stays there.
        }
        FindCurrentSize();     //this copies new font size to the currentSize,  raises the event 
    }

    /// <summary>
    /// This copies the actual fontsize to the currentSize, 
    /// depending on the new TextSizeCategory (M,L,S,etc)
    /// then calls the event (raises the alarm)
    /// </summary>
    private void FindCurrentSize()
    {
        /// Sets the actual currentSize
        switch (currentTextSizeCategory)
        {
            case TextSizeCategory.XL:
                currentSize = sizeXL;
                break;
            case TextSizeCategory.L:
                currentSize = sizeL;
                break;
            case TextSizeCategory.M:
                currentSize = sizeM;
                break;
            case TextSizeCategory.S:
                currentSize = sizeS;
                break;
            case TextSizeCategory.XS:
                currentSize = sizeXS;
                break;
        }

        //textFields.ForEach(element => element.fontSize = currentSize); //lo quité cuando pasé a delegates

        /// calls the event
        /// When I change size, I fire the OnThetSizeChanger event "alarm" message (calls the event).
        OnTextSizeChanged?.Invoke(currentSize);
        //if (OnTextSizeChanged != null)
        //    OnTextSizeChanged(currentSize);        
        
    }

    /// <summary>
    /// set the font size in editor
    /// </summary>
    private void OnValidate()
    {
        FindCurrentSize();
    }
}

