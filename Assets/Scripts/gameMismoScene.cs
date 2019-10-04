using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameMismoScene : MonoBehaviour
{
    public int homeMenuIndexNumber;
    private int sceneIndex;
    // Start is called before the first frame update
    void Start(){
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(homeMenuIndexNumber);
        }
    }
}
