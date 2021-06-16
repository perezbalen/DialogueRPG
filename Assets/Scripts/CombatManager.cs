using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;

public class CombatManager : MonoBehaviour
{
    #region parameters
    [Header("Dialogue System stuff")]
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;

    [Header("Players with Player Sheets")]
    public PlayerSRPGSheet Player1;
    public PlayerSRPGSheet Player2;

    public DiceRoller diceRoller;

    public CardDisplay cardDisplay;

    public enum StatElement { Stamina, Technique, Charisma, Will }

    #endregion

    /// <summary>
    /// Returns true if the Player rolling won.
    /// </summary>
    /// <param name="stat">what Stat is the "Attack/action" using</param>
    /// <param name="numberToBeatOrMatch">the dificulty of the action in number form</param>
    /// <param name="PlayerRolling">The one doing hte action. Usually the player</param>
    /// <param name="PlayerDefending">Usually the NPC</param>
    /// <returns>result of the roll</returns>
    public int RollBetweenTwoPlayers(StatElement stat, int numberToBeatOrMatch,
        PlayerSRPGSheet PlayerRolling = null, PlayerSRPGSheet PlayerDefending = null)
    {
        //Deal with optional parameters.
        if (PlayerRolling == null)
            PlayerRolling = Player1;
        if (PlayerDefending == null)
            PlayerDefending = Player2;

        //Roll the dice.
        int modifier = CalculateModifier(PlayerRolling, PlayerDefending, stat);
        return diceRoller.RollDiceResult(numberToBeatOrMatch, modifier);
    }

    /// <summary>
    /// Returns true if the Player rolling won.
    /// </summary>
    /// <param name="stat">what Stat is the "Attack/action" using</param>
    /// <param name="numberToBeatOrMatch">the dificulty of the action in number form</param>
    /// <param name="PlayerRolling">The one doing hte action. Usually the player</param>
    /// <param name="PlayerDefending">Usually the NPC</param>
    /// <returns>Did PlayerRolling win?</returns>
    public bool RollBetweenTwoPlayersBool(StatElement stat, int numberToBeatOrMatch, 
        PlayerSRPGSheet PlayerRolling = null, PlayerSRPGSheet PlayerDefending = null)
    {
        int roll = RollBetweenTwoPlayers(stat, numberToBeatOrMatch, PlayerRolling, PlayerDefending);
        return (roll >= numberToBeatOrMatch) ? true : false;
    }

    //use this from dialoge system,
    public int RollDiceResult(int numberToBeatOrMatch = 5)
    {
        // # deremine stat
        StatElement statAtPlay = StatElement.Charisma; //change this for code that detemines the stat at play

        // # Roll dice w mod
        int rollResult = RollBetweenTwoPlayers(statAtPlay, numberToBeatOrMatch);

        bool isItAWinScenario = rollResult >= numberToBeatOrMatch ? true : false;

        // # update sheets

        #region with winner and looser
        /*
        // ## Determinate winer and looser.
        PlayerSRPGSheet PlayerWinner;
        PlayerSRPGSheet PlayerLooser;
        if (isItAWinScenario) //Player 1 won roll
        {
            PlayerWinner = Player1;
            PlayerLooser = Player2;
        }
        else //Player 2 won roll
        {
            PlayerWinner = Player2;
            PlayerLooser = Player1;
        }

        // # Increase Exhaustion
        // these are events. Maybe use them differenty?
        PlayerWinner.ChangeExhaustion(12 - PlayerWinner.stamina); //Find a better way. Not a constant.
        PlayerLooser.ChangeExhaustion(12 - PlayerLooser.stamina); //Probably get this falue from ActionScene. Different in each case.

        if (isItAWinScenario)
        {
            PlayerWinner.ChangeArousal(12 - PlayerWinner.technique);
            PlayerLooser.ChangeArousal(12 - PlayerLooser.technique);  //just some random math. Re-think all this better.
        }
        else //loose scenario
        {
            //Player 2 gets mad
            PlayerWinner.ChangeFrustration(12 - PlayerWinner.will); // find better way/
            PlayerLooser.ChangeArousal( (12 - PlayerWinner.technique) / 2);  //just some random math. Re-think all this better.
        }
        */
        #endregion


        // # read the current Card
        Card card = cardDisplay.currentCard;

        // # Do stuff when isItAWinScenario
        if (isItAWinScenario)
        {
            // Player 1
            Player1.ChangeExhaustion(   card.exhaustionHitPlayer1Success    );//Player1.stamina);
            Player1.ChangeFrustration(  card.frustrationHitPlayer1Success);//* Player1.will);
            Player1.ChangeArousal(      card.arousalHitPlayer1Success);//* Player1.technique);
            // Player 2
            Player2.ChangeExhaustion(   card.exhaustionHitPlayer2Success);//   * Player2.stamina);
            Player2.ChangeFrustration(  card.frustrationHitPlayer2Success);//* Player2.will);
            Player2.ChangeArousal(      card.arousalHitPlayer2Success);//* Player2.technique);
        }
        else // # Do stuff wne !isItAWinScenario
        {
            Player1.ChangeExhaustion(   card.exhaustionHitPlayer1Failure);//* Player1.stamina);
            Player1.ChangeFrustration(  card.frustrationHitPlayer1Failure);//* Player1.will);
            Player1.ChangeArousal(      card.arousalHitPlayer1Failure);//* Player1.technique);
            // Player 2
            Player2.ChangeExhaustion(   card.exhaustionHitPlayer2Failure);//* Player2.stamina);
            Player2.ChangeFrustration(  card.frustrationHitPlayer2Failure);//* Player2.will);
            Player2.ChangeArousal(      card.arousalHitPlayer2Failure);//* Player2.technique);
        }

        // # update gui bars is done by the sheerts.

        // # deal with bars at max.
        // # deal with bars at waring.

        return rollResult;
    }

    /// <summary>
    /// Calculates the modifier for a roll
    /// The bounus or penalty.
    /// </summary>
    /// <param name="PlayerRolling"></param>
    /// <param name="PlayerDefending"></param>
    /// <param name="stat">enum StatElement { Stamina, Technique, Charisma, Will }</param>
    /// <returns>number to add to the dice roll (can be negative for penaly)</returns>
    public static int CalculateModifier (PlayerSRPGSheet PlayerRolling, PlayerSRPGSheet PlayerDefending, StatElement stat)
    {
        int rollModifier = 0;
        switch (stat) {
            case StatElement.Stamina:
                rollModifier = PlayerRolling.stamina - PlayerDefending.stamina;
                break;
            case StatElement.Technique:
                rollModifier = PlayerRolling.technique - PlayerDefending.technique;
                break;
            case StatElement.Charisma:
                rollModifier = PlayerRolling.charisma - PlayerDefending.charisma;
                break;
            case StatElement.Will:
                rollModifier = PlayerRolling.will - PlayerDefending.will;
                break;
        }
        return rollModifier;

    }


    #region Lua communication Fusntions. 
    //Lua needs numbers as doubles.
    //be shure to deal with that.
    public bool RollDice(double numberToBeatOrMatch)
    {
        //implement logic. true if beats it inclusive,
        //Fix the hardoded use of charisma.
        //not sure if this funtion will be needed.
        return RollBetweenTwoPlayersBool(StatElement.Charisma, (int)numberToBeatOrMatch);
    }

    public double RollDiceResult(double numberToBeatOrMatch)
    {
        //implement logic. true if beats it inclusive,
        return (double)RollDiceResult((int)numberToBeatOrMatch);
    }

    
    //get the number of a current stat (not done)
    public double GetStat(string statName)
    {
        // NOT DONE
        //change this to use enumerator isntead of string
        return 10d;
    }
    

    //Modify the number of a stat (not done)
    //dont need it, but shows how to LUA two values.
    public void UpdateStat(string statName, double valueToAdd)
    {
        // NOT DONE
        //change this to use enumerator isntead of string
        Player1.ChangeExhaustion ( (int)valueToAdd );
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


}
