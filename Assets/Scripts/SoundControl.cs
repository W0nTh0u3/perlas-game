using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioSource backgroundSound;
    public AudioSource clickSound;
    // Start is called before the first frame update
    void Start()
    {
        //Make sure the entry on the Prefs exist or proceed to create it
        if (PlayerPrefs.HasKey("MusicOpt"))
        {
            //Check the value of the Pref as a toggle (1 = max volume, 0 = off)
            backgroundSound.mute = (PlayerPrefs.GetInt("MusicOpt",1) == 0) ? true : false;
        }
        else
        {
            PlayerPrefs.SetInt("MusicOpt", 1);
        }
        //Make sure the entry on the Prefs exist or proceed to create it
        if (PlayerPrefs.HasKey("SoundOpt"))
        {
            //Check the value of the Pref as a toggle (1 = max volume, 0 = off)
            clickSound.mute = (PlayerPrefs.GetInt("SoundOpt",1) == 0) ? true : false;
        }
        else
        {
            PlayerPrefs.SetInt("SoundOpt", 1);
            
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        backgroundSound.mute = (PlayerPrefs.GetInt("MusicOpt",1) == 0) ? true : false;
        clickSound.mute = (PlayerPrefs.GetInt("SoundOpt",1) == 0) ? true : false;
    }
}
