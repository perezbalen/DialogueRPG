using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPanel : MonoBehaviour
{
    [Header("Player/NPC with stat sheet")]
    public PlayerSRPGSheet playerSheet;
  
    [Header("This panel's texts")]
    public TextMeshProUGUI staminaValue;
    public TextMeshProUGUI techniqueValue;
    public TextMeshProUGUI charismaValue;
    public TextMeshProUGUI willValue;

    // Start is called before the first frame update
    void Start()
    {
        ReadSheet();
    }

    // Update is called once per frame
    void OnValidate()
    {
        //ReadSheet();
    }

    /// <summary>
    /// Places on the panel, the values on the sheet
    /// </summary>
    private void ReadSheet()
    {
        staminaValue.text = playerSheet.stamina.ToString();
        techniqueValue.text = playerSheet.technique.ToString();
        charismaValue.text = playerSheet.charisma.ToString();
        willValue.text = playerSheet.will.ToString();
    }
}
