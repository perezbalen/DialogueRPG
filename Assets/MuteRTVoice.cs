using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using TMPro;

public class MuteRTVoice : MonoBehaviour
{
    public Speaker speaker;
    public TextMeshProUGUI label;
    // Start is called before the first frame update

    private void Start()
    {
        //Start muted. Just for development. 
        //remove when finisred.
        speaker.Mute();
    }

    public void MuteOrUnmuteSwitch()
    {
        speaker.MuteOrUnMute();
        label.text = speaker.isMuted ? "Unmute" : "Mute";
    }
}
