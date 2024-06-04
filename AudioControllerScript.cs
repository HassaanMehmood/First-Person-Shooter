using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerScript : MonoBehaviour
{
    // Hold  a refernce mute and unmute gameobjects from inspector.
    public GameObject mutebutton;
    public GameObject unmutebutton;
    // public AudioSource muteaudio;
    public static bool muteflag;

    // Start is called before the first frame update
    void Start()
    {
        if (muteflag == true)
        {
            AudioListener.volume = 0;
            unmutebutton.SetActive(false);
        }
        if (muteflag == false)
        {
            AudioListener.volume = 1;
            unmutebutton.SetActive(false);
        }
    }
    
    public void mutegame()
    {
        AudioListener.volume = 0;
        mutebutton.SetActive(false);
        unmutebutton.SetActive(true);
        muteflag = true;
     }

    public void unmutegame()
    {
        if (muteflag == true)
        {
            AudioListener.volume = 1;
            mutebutton.SetActive(true);
            unmutebutton.SetActive(false);
            muteflag = false;
        }
    }
}
