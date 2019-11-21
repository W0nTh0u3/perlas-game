using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle MusicOpt;
    public Toggle SoundOpt;
    // Start is called before the first frame update
    void Start()
    {
        MusicOpt.isOn = (PlayerPrefs.GetInt("MusicOpt", 1) == 1) ? true : false;
        SoundOpt.isOn = (PlayerPrefs.GetInt("SoundOpt", 1) == 1) ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicOpt.isOn)
            PlayerPrefs.SetInt("MusicOpt", 1);
        else
            PlayerPrefs.SetInt("MusicOpt", 0);
        if (SoundOpt.isOn)
            PlayerPrefs.SetInt("SoundOpt", 1);
        else
            PlayerPrefs.SetInt("SoundOpt", 0);

    }
}
