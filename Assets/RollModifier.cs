using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollModifier : MonoBehaviour
{
    [Header("Connectors")]
    [Tooltip("Where shoud we display the number? A TextMeshPro.")]
    public TextMeshProUGUI textWithModifier;

    public float fadeSpeed = 1f;

    private void Start()
    {
        
    }
    public void ShowModifierInGui (int modifier)
    {
        StartCoroutine(FadeTextToFullAlpha(fadeSpeed, textWithModifier));

        string stringValue = (modifier > 0) ? "+" + modifier : modifier.ToString();
        textWithModifier.text = stringValue;
    }

    #region Fade animation
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
    #endregion
}
