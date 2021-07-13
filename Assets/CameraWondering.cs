using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWondering : MonoBehaviour
{
    public Vector3 cameraOrigin;
    public Vector3 cameraDestination;
    [Tooltip("For 2D, use Z as Zoom")]
    public bool zAsZoom;
    [Header("Curves")]
    public LeanTweenType easeTypeX;
    public LeanTweenType easeTypeY;
    public LeanTweenType easeTypeZ;
    [Header("Time")]
    public Vector3 axisTime;
    

    // Start is called before the first frame update

    void Start()
    {
        gameObject.transform.position = cameraOrigin; 
    }

    void OnEnable()
    {
        LeanTween.moveX(gameObject, cameraDestination.x, axisTime.x).setLoopPingPong().setEase(easeTypeX);
        LeanTween.moveY(gameObject, cameraDestination.y, axisTime.y).setLoopPingPong().setEase(easeTypeY);
        //For 2D, Z move does nothing. So the trick is to scale the object X, Y, and Z (which, again, does nothing)
        if (zAsZoom)
        {
            Vector3 OriginalSize = gameObject.transform.localScale;
            LeanTween.scale(gameObject, OriginalSize*cameraDestination.z, axisTime.z).setLoopPingPong().setEase(easeTypeZ);
        }
        else
        {
            LeanTween.moveZ(gameObject, cameraDestination.z, axisTime.z).setLoopPingPong().setEase(easeTypeZ);
        }
    }

    // Update is called once per frame

}
