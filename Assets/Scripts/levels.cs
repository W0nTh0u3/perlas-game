using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levels : MonoBehaviour {
    public int[] levelUnlock = new int[10];
    public int[] levelStar = new int[10];
    public bool timeMode;
    public int highScore;
    public Sprite[] levelStarDisplay;
    public Image[] levelNumberS;
    public Button[] levelBtnSelection = new Button[10];
    private float x = 1f;
    private readonly RuntimePlatform platform = Application.platform;
    // Start is called before the first frame update
    void Start () {
        dataC data = saveSystem.LoadData ();
        if (data != null) {
            levelUnlock = data.levelUnlock;
            levelStar = data.levelStar;
            highScore = data.highScore;
            timeMode = data.timeMode;
        }

        Object[] stars = Resources.LoadAll ("UI/stars", typeof (Sprite));
        levelStarDisplay = new Sprite[stars.Length];
        for (int x = 0; x < stars.Length; x++) {
            levelStarDisplay[x] = (Sprite) stars[x];
        }
        for (int x = 0; x < levelUnlock.Length; x++) {
            if (levelStar[x] == 0)
                levelNumberS[x].sprite = levelStarDisplay[0];
            else if (levelStar[x] == 1)
                levelNumberS[x].sprite = levelStarDisplay[1];
            else if (levelStar[x] == 2)
                levelNumberS[x].sprite = levelStarDisplay[2];
            else if (levelStar[x] == 3)
                levelNumberS[x].sprite = levelStarDisplay[3];
        }
        foreach (var button in levelBtnSelection)
        {
            button.interactable = false;
        }
        for (int z=0; z < levelBtnSelection.Length; z++)
        {
            if (levelUnlock[z] == 0)
                levelBtnSelection[z].image.color = new Color32(125,125,125,125);
        }
    }

    // Update is called once per frame
    void Update () {
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (x <= 0)
            {
                EnableAllBtns();
            }
            else
                x -= Time.deltaTime;
        }
        else
            EnableAllBtns();
    }
    private void EnableAllBtns()
    {
        for (int z = 0; z < levelBtnSelection.Length; z++)
        {
            if (levelUnlock[z] == 1)
                levelBtnSelection[z].interactable = true;
        }
    }
}