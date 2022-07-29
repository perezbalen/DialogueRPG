using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class d6Dice : MonoBehaviour
{
    [Header("Dice parameters")]
    public int maxValue = 6;
    public float minFadeSpeed = 0.1f;
    public float maxFadeSpeed = 3f;
    [Header("Connect to game object")]
    public TextMeshProUGUI diceFaceText;

    [SerializeField]
    private Sprite dice1;
    [SerializeField]
    private Sprite dice2;
    [SerializeField]
    private Sprite dice3;
    [SerializeField]
    private Sprite dice4;
    [SerializeField]
    private Sprite dice5;
    [SerializeField]
    private Sprite dice6;

    private Image diceFace;

    private void Start()
    {
        diceFace = gameObject.GetComponentInChildren<Image>();
    }
    private void OnValidate()
    {
        //diceFaceText.SetText("" + maxValue);
    }


    public int RollDice()
    {
        StartCoroutine(FadeTextToFullAlpha( Random.Range(minFadeSpeed, maxFadeSpeed), diceFaceText));

        int result = Random.Range(1, maxValue + 1);
        //diceFaceText.SetText("" + result);
        showDiceFace(result);
        return result;
    }

    private void showDiceFace(int value)
    {
        switch (value)
        {
            case 1:
                diceFace.sprite = dice1;
                break;
            case 2:
                diceFace.sprite = dice2;
                break;
            case 3:
                diceFace.sprite = dice3;
                break;
            case 4:
                diceFace.sprite = dice4;
                break;
            case 5:
                diceFace.sprite = dice5;
                break;
            case 6:
                diceFace.sprite = dice6;
                break;
            default:
                Debug.Log("Dice roll out of bounds: "+ value);
                break;
        }
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
