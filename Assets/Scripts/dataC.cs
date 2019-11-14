using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class dataC {
    public int[] levelUnlock = new int[10];
    public int[] levelStar = new int[10];
    public bool timeMode;
    public int highScore;
    public dataC (Data Levels) {
        levelUnlock = Levels.levelUnlock;
        levelStar = Levels.levelStar;
        timeMode = Levels.timeMode;
        highScore = Levels.highScore;
    }
}