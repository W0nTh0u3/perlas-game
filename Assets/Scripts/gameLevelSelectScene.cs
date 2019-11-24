using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameLevelSelectScene : MonoBehaviour {
    public int loadSceneNumber;
    public int homeMenuIndexNumber;
    public int levelSelectIndexNumber;
    private int sceneIndex;
    // Start is called before the first frame update
    void Start () {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {   
            if (sceneIndex != levelSelectIndexNumber)
            {
                Debug.Log(PlayerPrefs.GetInt("levelToLoad"));
                if (PlayerPrefs.GetInt("levelToLoad") == 0)
                    PlayerPrefs.SetInt("sceneToLoad", homeMenuIndexNumber);
                else
                    PlayerPrefs.SetInt("sceneToLoad", levelSelectIndexNumber);
                SceneManager.LoadScene (loadSceneNumber);
            }
        }
    }
}