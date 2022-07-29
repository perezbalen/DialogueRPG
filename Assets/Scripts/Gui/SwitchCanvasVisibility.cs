using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchCanvasVisibility : MonoBehaviour
{
    public delegate void AskedToHide();
    public static event AskedToHide OnAskedToHide;
    public delegate void AskedToShow();
    public static event AskedToShow OnAskedToShow;

    //presumo que arrancamos visibles.
    private static bool isVisible = true;
    private TextMeshProUGUI label;

    public void VisibleOrInvisibleSwitch()
    {
        label = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        label.text = isVisible ? "Show GUI" : "Hide GUI"; //soy visible y me paso a invisible

        if (isVisible)
            OnAskedToHide?.Invoke();
        else
            OnAskedToShow?.Invoke();

        isVisible = !isVisible;
    }


}
