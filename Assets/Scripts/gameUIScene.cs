using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameUIScene : MonoBehaviour
{
    public int timeAttackSceneNumber;
    public int classicModeSceneNumber;
    private int sceneIndex;
    // Start is called before the first frame update
    void Start(){
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    public void LoadNextLevel(){
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadTimeAttackGame(){
        SceneManager.LoadScene(timeAttackSceneNumber);
    }

    public void LoadClassicModeGame(){
        SceneManager.LoadScene(classicModeSceneNumber);
    }
}
