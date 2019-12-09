using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOnlyScene : MonoBehaviour
{
    public GameObject quitCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitCanvas.SetActive(true);
        }
    }
    public void QuitGameNow()
    {
        Application.Quit();
        Debug.Log("game quit.");
    }
}
