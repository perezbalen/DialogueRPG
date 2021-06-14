using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManagerTester : MonoBehaviour
{
    public CombatManager combatManager;

    public void TestCharimsaAttack()
    {
        combatManager.RollDiceResult(7);
    }
}
