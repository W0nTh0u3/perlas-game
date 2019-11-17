using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameUIScene : MonoBehaviour {
    public int LevelSelection;
    public int timeAttackSceneNumber;
    public int classicModeSceneNumber;
    public int homeSceneNumber;
    public int loadSceneNumber;
    private int sceneIndex;
    // Start is called before the first frame update
    void Start () {
        sceneIndex = SceneManager.GetActiveScene ().buildIndex;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (sceneIndex != classicModeSceneNumber)
                Application.Quit();
            else
                LoadHomeScreen();
        }
    }

    public void LoadNextLevel () {
        SceneManager.LoadScene (sceneIndex);
    }

    public void LoadTimeAttackGame () {
        PlayerPrefs.SetInt("levelToLoad", 0);
        //SceneManager.LoadScene(timeAttackSceneNumber);
        BoolPrefs.SetBool("isTimeAttack",true);
        PlayerPrefs.SetInt ("sceneToLoad", timeAttackSceneNumber);
        SceneManager.LoadScene (loadSceneNumber);
    }
    public void LevelSelectionNumberButton(int i)
    {
        LevelSelection = i;
    }

    public void LoadGameWithLevel()
    {
        PlayerPrefs.SetInt("levelToLoad",LevelSelection);
        BoolPrefs.SetBool("isTimeAttack", false);
        PlayerPrefs.SetInt("sceneToLoad", timeAttackSceneNumber);
        SceneManager.LoadScene(loadSceneNumber);
    }

    public void LoadClassicModeGame () {
        PlayerPrefs.SetInt ("sceneToLoad", classicModeSceneNumber);
        SceneManager.LoadScene (loadSceneNumber);
    }

    public void LoadHomeScreen () {
        PlayerPrefs.SetInt ("sceneToLoad", homeSceneNumber);
        SceneManager.LoadScene (loadSceneNumber);
    }

    public void introHomeScreen () {
        SceneManager.LoadScene (homeSceneNumber);
    }
}