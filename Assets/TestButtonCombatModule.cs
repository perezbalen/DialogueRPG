using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestButtonCombatModule : MonoBehaviour
{
    public CombatManager combatManager;
    public CombatManager.StatElement stat;
    public int dificultyNumber;

    public void ButtonClicked()
    {
        combatManager.RollBetweenTwoPlayers(stat, dificultyNumber);
    }
}
