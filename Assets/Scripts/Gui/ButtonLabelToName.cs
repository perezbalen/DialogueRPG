using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLabelToName : MonoBehaviour
{
    private void OnValidate()
    {
        gameObject.GetComponentInChildren<Text>().text = gameObject.name;
    }
}
