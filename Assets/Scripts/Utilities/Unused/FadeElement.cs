using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeElement : MonoBehaviour
{
      private TextMeshProUGUI textMesh;
      public float speed=0.5f;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        //Apparently this happens the first time and OnEnabled, every othet time. So
        MakeTextInvisible();
    }

    //Apparetly this happens only the second time there's text
    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        MakeTextInvisible();
    }

    //Apparetly this happens only the second time there's text
    private void OnDisable()
    {
        //textMesh = GetComponent<TextMeshProUGUI>();
        //MakeVisible(false);
    }

    // can ignore the update, it's just to make the coroutines get called for example
    void Update()
    {
        //for debug
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeTextToFullAlpha(speed, textMesh));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeTextToZeroAlpha(speed, textMesh));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MakeVisible(false);
        }

    }
    private void MakeTextInvisible()
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
        StartCoroutine(FadeTextToFullAlpha(speed, textMesh));
    }

    //instant turn visible or invisible
    public void MakeVisible(bool visible)
    {
        if (visible)
        {
            //Debug.Log("Enter Make Vissible");
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);
        }
        else
        {
            //Debug.Log("Enter Make InVissible");
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
        }
    }

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


}
