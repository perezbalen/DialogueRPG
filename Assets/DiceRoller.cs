using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using MoreMountains.Feedbacks;

public class DiceRoller : MonoBehaviour
{
    #region Parameters
    [Header("Interpretation terms")]
    public string failure = "Failure";
    public Color colorFail;
    public string success = "Success";
    public Color colorSuccess;
    public float fadeSpeed = 2f;
    public float waitBeforeFade = 2f;

    [Header("Connectors")]
    public d6Dice dice1;
    public d6Dice dice2;
    public RollModifier rollModifierText;
    public TextMeshProUGUI resultLabel;
    public MMFeedbacks feedbacks;

    [Header("Debug")]
    public int numberToBeat = 6;
    public int debugResult;

    [Header("Events")]
    public UnityEventInt OnDiceRolledWithResult;
    public UnityEventBool OnDiceRolled;



    #endregion

    /// <summary>
    /// Rolls the dice. Dispalys nubers and Success/Failure images.
    /// Returns the mumber you got.
    /// </summary>
    /// <param name="numberToBeatOrMatch"></param>
    /// <param name="modifier">PlayerStat - EnemyStat = modifier</param>
    /// <returns></returns>
    public int RollDiceResult(int numberToBeatOrMatch, int modifier = 0)
    {
        //play sound
        PlayRollingSound();

        int result1 = dice1.RollDice();
        int result2 = dice2.RollDice();
        rollModifierText.ShowModifierInGui(modifier);
        
        int result = result1 + result2 + modifier;

        if (result >= numberToBeatOrMatch)
        {
            // does GUI stuff
            IsSuccess(true);
            
            //Tells all observers the result
            OnDiceRolledWithResult.Invoke(result);

            //tells whoever called the function, the resutl
            return result;
        }
        else
        {
            IsSuccess(false);
            OnDiceRolledWithResult.Invoke(result);
            return result;
        } 
    }

    /// <summary>
    /// Rolls the dice. Dispalys nubers and Success/Failure images.
    /// Returns true if succeded.
    /// </summary>
    /// <param name="numberToBeatOrMatch"></param>
    /// <returns></returns>
    public bool RollDice(int numberToBeatOrMatch)
    {

        //dontt play sound here, it's played on the onDiceResult()

        if (RollDiceResult(numberToBeatOrMatch) >= numberToBeatOrMatch) 
        {
            // Tells all observers that Player won the roll
            OnDiceRolled.Invoke(true);
            //tells whoever called the function, Player won
            return true; 
        }
        else 
        {
            OnDiceRolled.Invoke(false);
            return false; 
        }            
}

    public void TestOnRolledDice(int numberToBeatOrMatch)
    {
        Debug.Log("Enter the test:" + numberToBeatOrMatch);
    }

    /// <summary>
    /// Gui thing. Paints the screen with data
    /// </summary>
    /// <param name="isWin"></param>
    private void IsSuccess(bool isWin)
    {
        GetComponent<MakeVisibleWhenRolling>().SetVisible();

        StartCoroutine(FadeTextToFullAlpha(fadeSpeed, resultLabel));
        if (isWin)
        {
            resultLabel.SetText(success);
            resultLabel.color = colorSuccess;
        }
        else
        {
            resultLabel.SetText(failure);
            resultLabel.color = colorFail;
        }

        GetComponent<MakeVisibleWhenRolling>().SetInvisible();
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        yield return new WaitForSeconds(waitBeforeFade);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        yield return new WaitForSeconds(waitBeforeFade);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private void PlayRollingSound()
    {
        //gameObject.GetComponent<AudioSource>().Play();
        feedbacks?.PlayFeedbacks();
    }
}
