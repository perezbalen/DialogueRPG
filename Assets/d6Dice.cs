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
    public TextMeshProUGUI diceFace;

    private void OnValidate()
    {
        diceFace.SetText("" + maxValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        RollDice();
    }


    // Update is called once per frame
    void Update()
    {
        //for debug
        if(Input.GetKeyDown(KeyCode.R))
        {
            RollDice();
        }
    }

    public int RollDice()
    {
        StartCoroutine(FadeTextToFullAlpha( Random.Range(minFadeSpeed, maxFadeSpeed), diceFace));

        int result = Random.Range(1, maxValue + 1);
        diceFace.SetText("" + result);
        return result;
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
