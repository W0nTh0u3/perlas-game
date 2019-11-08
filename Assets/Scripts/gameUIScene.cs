using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameUIScene : MonoBehaviour {
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
            Application.Quit ();
        }
    }

    public void LoadNextLevel () {
        SceneManager.LoadScene (sceneIndex);
    }

    public void LoadTimeAttackGame () {
        //SceneManager.LoadScene(timeAttackSceneNumber);
        PlayerPrefs.SetInt ("sceneToLoad", timeAttackSceneNumber);
        SceneManager.LoadScene (loadSceneNumber);
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