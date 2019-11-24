using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameMismoScene : MonoBehaviour {
    public int loadSceneNumber;
    public int homeMenuIndexNumber;
    public int levelSelectIndexNumber;
    public int timeAttackSceneNumber;
    private int sceneIndex;
    // Start is called before the first frame update
    void Start () {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    public void ReturnToSelect()
    {
        Debug.Log(PlayerPrefs.GetInt("levelToLoad"));
        if (PlayerPrefs.GetInt("levelToLoad") == 0)
            PlayerPrefs.SetInt("sceneToLoad", homeMenuIndexNumber);
        else
            PlayerPrefs.SetInt("sceneToLoad", levelSelectIndexNumber);
        SceneManager.LoadScene(loadSceneNumber);
    }

    public void PlayAgain()
    {
        PlayerPrefs.SetInt("sceneToLoad", timeAttackSceneNumber);
        SceneManager.LoadScene(loadSceneNumber);
    }

    public void LoadNextLevelGame()
    {
        int NextLevel = PlayerPrefs.GetInt("levelToLoad") + 1;
        PlayerPrefs.SetInt("levelToLoad", NextLevel);
        BoolPrefs.SetBool("isTimeAttack", false);
        PlayerPrefs.SetInt("sceneToLoad", timeAttackSceneNumber);
        SceneManager.LoadScene(loadSceneNumber);
    }
}