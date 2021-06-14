using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    #region Parameters
    [Header("Cosmetic")]
    public string label;
    public Gradient gradient;

    [Header("Animation")]
    public bool shouldAnimate = true;
    public bool shouldNumbersAnimate = false;

    [Header("Animation Bounce")]
    [Range(0, 2)]
    public float bounceTime;
    [Range(0, 1)]
    public float bounceDelay;
    [Range(0, 3)]
    public float zoomAmount;
    public LeanTweenType bounceEaseType;

    [Header("Animation Slide")]
    [Range(0, 10)]
    public float slideTime;
    [Range(0, 1)]
    public float slideDelay;
    public LeanTweenType slideEaseType;

    [Header("Values")]
    public int minValue=0;
    public int maxValue=100;
    [Range(0, 100)]
    public int value;

    [Header("Connectors")]
    public Slider slider;
    public Image fill;
    public TextMeshProUGUI textLabel;
    public TextMeshProUGUI textValues;

    #endregion

    void OnValidate()
    {
        slider = gameObject.GetComponent(typeof(Slider)) as Slider;

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = value;
        SetValue(value);
        SetLabel(label);
        //Debug.Log("Modifying Inspector. OnValidate called.");
    }

    void Start ()
    {
        slider = gameObject.GetComponent(typeof(Slider)) as Slider;

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = value;
        SetValue(value);
        SetLabel(label);
    }

    public void SetLabel (string newLabel)
    {
        label = newLabel;
        textLabel.SetText(label);
    }

    #region Set_Value

    /// <summary>
    /// Sets the position of the bar
    /// </summary>
    /// <param name="newValue">the position of the bar.</param>
    public void SetValue (int newValue)
    {
        //cap at caps
        if (newValue >= maxValue) 
            { newValue = maxValue; }
        if (newValue <= minValue)
            { newValue = minValue; }

        //assign new value
        //slider.value = newValue; //this now done in the AnimateBarMovement
        AnimateBarMovement((float)value, (float)newValue);
        value = newValue;

        //If I want numbers NOT to animate, the update must be done here
        if (!shouldNumbersAnimate) {
            textValues.SetText(value + "/" + maxValue); }          
    }

    /// <summary>
    /// Animates the movement of the slier
    /// https://easings.net/#
    /// 
    /// values have to be float to animates well.
    /// </summary>
    /// <param name="oldValue">starting value</param>
    /// <param name="newValue">ending value</param>
    private void AnimateBarMovement(float oldValue, float newValue)
    {
        //Only animate if not in editor, OR if there is an actual change in values.
        if (!Application.isPlaying || oldValue == newValue)
        {
            UpdateBarValue(newValue);
            //Debug.Log("Modifying without Leantween inside AnimateBarMovement.");
        }
        else
        { 
            transform.localScale = Vector3.one;
            LeanTween.cancel(gameObject);

            LeanTween.scale(gameObject, Vector3.one * zoomAmount, bounceTime)
                .setDelay(bounceDelay)
                .setEase(bounceEaseType)
                .setLoopOnce();
             
            LeanTween.value(gameObject, UpdateBarValue, oldValue, newValue, slideTime)
                .setDelay(slideDelay)
                .setEase(slideEaseType);
        }
 
    }

    /// <summary>
    /// Moves the sliedr bar 
    /// Function needed fot the LeanTween.value
    /// But also used to bypass the LeanTween when no animation wanted.
    /// </summary>
    /// <param name="updatingValue"></param>
    public void UpdateBarValue(float updatingValue)
    {
        slider.value = updatingValue; 
        fill.color = gradient.Evaluate(slider.normalizedValue);

        //If I want numbers to animate, the update must be done here, and Not on SetValue
        if (shouldNumbersAnimate) {
            textValues.SetText((int)updatingValue + "/" + maxValue); }
    }
    #endregion
}
