using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneLoader : MonoBehaviour {

    private int scene;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text loadingText;
    // Updates once per frame
    void Start () {
        StartCoroutine (LoadNewScene ());
    }
    void Update () {

        // If the player has pressed the space bar and a new scene is not loading yet...

        // ...set the loadScene boolean to true to prevent loading a new scene more than once...

        // ...change the instruction text to read "Loading..."
        loadingText.text = "Loading...";

        // ...and start a coroutine that will load the desired scene.

        // If the new scene has started loading...

        // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
        loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));

    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene () {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds (5);
        scene = PlayerPrefs.GetInt ("sceneToLoad");

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
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

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        // while (!async.isDone) {
        //     yield return null;
        // }

    }

}