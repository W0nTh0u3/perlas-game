using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneLoader : MonoBehaviour {

    private int scene;
    public Image progressBar;
    public Text loadingText;
    // Updates once per frame
    void Start () {
        StartCoroutine (LoadNewScene ());
    }
    void Update () {
        loadingText.text = "Loading...";
        loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));

    }

    IEnumerator LoadNewScene () {
        yield return new WaitForSeconds (5);
        scene = PlayerPrefs.GetInt ("sceneToLoad");

        AsyncOperation async = SceneManager.LoadSceneAsync (scene);
        async.allowSceneActivation = false;
        // while (async.progress < 1){
        //     progressBar.fillAmount = async.progress;
        //     yield return new WaitForEndOfFrame();
        // }

        while (!async.isDone) {
            progressBar.fillAmount = async.progress;
            if (async.progress >= 0.9f) {
                progressBar.fillAmount = 1f;
                loadingText.text = "Done Loading\nTap to Continue";
                loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));
                if (Input.touchCount > 0 || Input.GetMouseButton (0)) {
                    async.allowSceneActivation = true;
                }
            }
            yield return null;
        }

    }

}