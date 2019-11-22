using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timeModePlay : MonoBehaviour {
    public Button timeModePlayBtn;
    public TextMeshProUGUI timeModeText;
    private bool isClassicDone;
    // Start is called before the first frame update
    void Start () {
        dataC data = saveSystem.LoadData ();
        if (data != null)
            isClassicDone = data.timeMode;
        else
        {

        }
        if (isClassicDone != true) {
            timeModePlayBtn.interactable = isClassicDone;
            timeModeText.color = new Color32 (255, 255, 255, 95);
            timeModeText.text = "Locked\n Zen Mode";
        }
    }
}