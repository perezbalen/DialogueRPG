using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;

/*
[System.Serializable]
public class UnityEventInt : UnityEvent<int> {}
[System.Serializable]
public class UnityEventFloat : UnityEvent<float> {}
[System.Serializable]
public class UnityEventString : UnityEvent<string> {}*/

public class PlayerSRPGSheet : MonoBehaviour
{
    #region Properties
    [Header("Dialogue System stuff")]
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false; //Maybe remove. If not needed.

    //energy levles
    [Header("Player HP")]
    public int exhaustionMaxLevel;
    [Range(0, 120)]
    public int exhaustionLevel;

    public int frustrationMaxLevel;
    [Range(0, 120)]
    public int frustrationLevel;

    public int arousalMaxLevel;
    [Range(0, 120)]
    public int arousalLevel;

    [Header("Player Stats")]
    [Range(2, 12)]
    public int stamina;
    [Range(2, 12)]
    public int will;
    [Range(2, 12)]
    public int technique;
    [Range(2, 12)]
    public int charisma;

    [Tooltip("Read only. This gets recalculated constantly.")]
    public int sumStats; //this is exposed because I might need it on char creation.

    //GUI exhaustionBar
    [Header("GUI Energy Bars. Connect to them")]
    [Tooltip("Connect this to the appropriate Energy bars")]
    public EnergyBar exhaustionBar;
    public EnergyBar frustrationBar;
    public EnergyBar arousalBar;

    //Dice set. Figure better way to do this.
    [Header("Scene dice set. (Figure better way to do this connection)")]
    public DiceRoller diceRoller;
    
    [Header("Events")]
    [Tooltip("Connect this to the appropriate Energy bars")]
    public UnityEventInt OnExhaustionChanged;
    public UnityEventInt OnFrustrationChanged;
    public UnityEventInt OnArousalChanged;
    #endregion

    /// <summary>
    /// Puts the information of this player sheet
    /// into the gui bars connected to it.
    /// </summary>
    private void SetUpGuiBars()
    {
        exhaustionBar.minValue = 0;
        exhaustionBar.maxValue = exhaustionMaxLevel;
        exhaustionBar.SetValue(exhaustionLevel);
        frustrationBar.minValue = 0;
        frustrationBar.maxValue = frustrationMaxLevel;
        frustrationBar.SetValue(frustrationLevel);
        arousalBar.minValue = 0;
        arousalBar.maxValue = arousalMaxLevel;
        arousalBar.SetValue(arousalLevel);
    }

    /// <summary>
    /// Shows the player sheet's data on the editor
    /// </summary>
    private void OnValidate()
    {
        CalculateSumStats();
        SetUpGuiBars();
        SetHealthPoolsMaxLevels();

    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealthPoolsMaxLevels();
        SetHealthPoolsInitialValues();

        /// Puts this player info in the GUI
        SetUpGuiBars();

        /// Registers the listeners
        if (OnExhaustionChanged == null) {
            OnExhaustionChanged = new UnityEventInt(); }
        if ( OnFrustrationChanged == null) {
            OnFrustrationChanged = new UnityEventInt(); }
        if ( OnArousalChanged == null) {
            OnArousalChanged = new UnityEventInt(); }

        
    }

    // Update is called once per frame
    void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ChangeExhaustion(-10);
            ChangeArousal(-10);
            ChangeFrustration(-10); 
        }
    }

    #region initialize sheet

    /// <summary>
    /// Sets the health pools mac value to the relevant stat * 10
    /// </summary>
    private void SetHealthPoolsMaxLevels()
    {
        exhaustionMaxLevel = stamina * 10;
        frustrationMaxLevel = will * 10;
        arousalMaxLevel = technique * 10;
    }

    /// <summary>
    /// Sets the Health pools at their ideal inicial values.
    /// </summary>
    private void SetHealthPoolsInitialValues()
    {
        exhaustionLevel = 0;
        frustrationLevel = 0;
        arousalLevel = 0;
    }

    #endregion

    /// <summary>
    /// Helper function. 
    /// Just adds all the stats, to help with game balance.
    /// Put the sum of all stats in sumStats so you can see the character's "power level".
    /// </summary>
    private void CalculateSumStats()
    {
        sumStats = stamina + technique + charisma + will;
    }

    /* #region Lua communication Fusntions. 
    //Lua needs numbers as doubles.
    //be shure to deal with that.
    public bool RollDice(double numberToBeatOrMatch)
    {
        //implement logic. true if beats it inclusive,
        return diceRoller.RollDice((int)numberToBeatOrMatch);
    }

    public double RollDiceResult(double numberToBeatOrMatch)
    {
        //implement logic. true if beats it inclusive,
        return (double)diceRoller.RollDiceResult((int)numberToBeatOrMatch);
    }

    //get the number of a current stat (not done)
    public double GetStat(string statName)
    {
        //change this to use enumerator isntead of string
        return 10d;
    }
    #endregion

    #region Register with Lua
    /// <summary>
    /// For registering with Lua an the Conversation System
    /// </summary>
    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction("RollDice", this, SymbolExtensions.GetMethodInfo(() => RollDice((double)0)));
        Lua.RegisterFunction("RollDiceResult", this, SymbolExtensions.GetMethodInfo(() => RollDiceResult((double)0)));
        Lua.RegisterFunction("GetStat", this, SymbolExtensions.GetMethodInfo(() => GetStat(string.Empty)));
        Lua.RegisterFunction("UpdateStat", this, SymbolExtensions.GetMethodInfo(() => UpdateStat(string.Empty, (double)0)));
        //Lua.RegisterFunction("AddItem", this, SymbolExtensions.GetMethodInfo(() => addItem      (string.Empty, (double)0)));
    }

    /// <summary>
    /// For registering with Lua an the Conversation System
    /// This comes form the documentation of Conversation System
    /// </summary>
    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction("RollDice");
            Lua.UnregisterFunction("RollDiceResult");
            Lua.UnregisterFunction("GetStat");
            Lua.UnregisterFunction("UpdateStat");
        }
    }
    #endregion
    */

    #region Events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ammount">How much to increase or decrease the energy</param>
    public void ChangeExhaustion(int ammount)
    {
        exhaustionLevel += ammount;

        //Caps at cap
        if (exhaustionLevel >= exhaustionMaxLevel)
        {
            exhaustionLevel = exhaustionMaxLevel;
        }
        if (exhaustionLevel <= 0)
        {
            exhaustionLevel = 0;
        }

        //Tells the Observers.
        OnExhaustionChanged.Invoke(exhaustionLevel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ammount">How much to increase or decrease the energy</param>
    public void ChangeFrustration(int ammount)
    {
        frustrationLevel += ammount;

        //Caps at cap
        if (frustrationLevel >= frustrationMaxLevel)
        {
            frustrationLevel = frustrationMaxLevel;
        }
        if (frustrationLevel <= 0)
        {
            frustrationLevel = 0;
        }

        OnFrustrationChanged.Invoke(frustrationLevel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ammount">How much to increase or decrease the energy</param>
    public void ChangeArousal(int ammount)
    {
        arousalLevel += ammount;

        //Caps at cap
        if (arousalLevel >= arousalMaxLevel)
        {
            arousalLevel = arousalMaxLevel;
        }
        if (arousalLevel <= 0)
        {
            arousalLevel = 0;
        }

        OnArousalChanged.Invoke(arousalLevel);
    }

    #endregion
}
