using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;



public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //private static LTDescr delay;

    [Header("Tooltip message")]
    public string header;
    [TextArea]
    public string content;

    [Header("Customization")]
    public float delayTime = 0.5f;

    private static LTDescr delay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(delayTime, () =>
        {
            TooltipSystem.Show(content, header);
        });
    }

    // Update is called once per frame
    public void OnPointerExit(PointerEventData eventData)
    {
        if (delay !=null)
            LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

    public void OnDisable()
    {
        //LeanTween.cancelAll();
        TooltipSystem.Hide();
        //Debug.Log("ondisable insdie the tooltip trigger.");
    }

    public void SetTooltipText(string newContent)
    {
        TextMeshProUGUI temp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        header = newContent;
        content = temp.text;

    }

}
