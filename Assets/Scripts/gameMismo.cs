using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;

public class gameMismo : MonoBehaviour {
    public Animator animationRedGreen;
    private Data LevelScore = new Data ();
    public Image drawPanel;
    public Button recognizeButton;
    public Transform gestureOnScreenPrefab;
    public Text drawLabel;
    public Text verifyLabel;
    public Text scoreLabel;
    public Text timeLabel;
    public Text countDownLabel;
    public GameObject pauseScreen;

    private List<Gesture> trainingSet = new List<Gesture> ();

    private List<Point> points = new List<Point> ();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea = new Rect (0, 0, Screen.width, Screen.height / 2);
    // private Rect boxArea = new Rect (0, Screen.height / 2, Screen.width, Screen.height / 2);

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer> ();
    private LineRenderer currentGestureLineRenderer;

    private Gesture randomGesture;
    private Gesture[] selections;
    private int Score = 0;

    //GUI
    private string message;
    private bool recognized;
    private bool GameOver = false;

    //private string newGestureName = "";
    private float timeLeft = 11f;
    private float countDownTime = 4f;
    private float timeForward = 0f;
    private bool isZenMode;
    private int numLevelClicked = 0;
    private Result gestureResult;
    void Start () {
        Debug.Log(Application.persistentDataPath);
        animationRedGreen.Play("DefaultDraw");
        isZenMode = BoolPrefs.GetBool ("isTimeAttack");
        if (isZenMode == false) {
            numLevelClicked = PlayerPrefs.GetInt ("levelToLoad");
        }
        dataC loadedData = saveSystem.LoadData ();
        Button btn = recognizeButton.GetComponent<Button> ();
        btn.onClick.AddListener (TaskOnClick);
        platform = Application.platform;
        //drawArea = new Rect(0, 0, Screen.width, Screen.height / 2);

        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset> ("GestureSet/baybayin/");
        foreach (TextAsset gestureXml in gesturesXml) {
            trainingSet.Add (GestureIO.ReadGestureFromXML (gestureXml.text));
        }
        selections = trainingSet.ToArray ();
        if (isZenMode == true)
            scoreLabel.text = "Score:\n" + Score;
        else
            scoreLabel.text = "";
        Shuffler ();
        drawLabel.text = "";
        countDownLabel.text = "";
        verifyLabel.text = "";
    }

    void Shuffler () {
        randomGesture = selections[UnityEngine.Random.Range (0, selections.Length)];
        Debug.Log (isZenMode);
        Debug.Log (numLevelClicked);
        //drawLabel.text = randomGesture.Name;
    }

    void Update () {
        if (countDownTime < 0) {
            if (isZenMode == true)
                ZenMode ();
            else
                ClassicMode ();

        } else {
            countDownTime -= Time.deltaTime;
            if (Mathf.RoundToInt (countDownTime) == 0)
                countDownLabel.text = "Draw!";
            else
                countDownLabel.text = Mathf.RoundToInt (countDownTime).ToString ();
        }
    }
    void ZenMode () {
        timeLeft -= Time.deltaTime;
        if (timeLeft >= 0) {
            countDownLabel.text = "";
            drawLabel.text = randomGesture.Name;
            timeLabel.text = Mathf.RoundToInt (timeLeft).ToString ();
            DrawMode ();
        } else {
            if (GameOver != true)
                GameOverScreen ();
        }
    }
    void ZenModeCheck () {
        if (gestureResult.Score > 0.9f) {
            float percentScore = (gestureResult.Score) * 100;
            animationRedGreen.Play("greenDraw",-1,0f);
            //verifyLabel.text = "Correct: " + percentScore.ToString () + " %";
            verifyLabel.text = "Correct";
            Score++;
            scoreLabel.text = "Score:\n" + Score;
            timeLeft += 10f;
            Shuffler ();
        } else {
            //if (Score > 0) {
                animationRedGreen.Play("redDraw",-1,0f);
                verifyLabel.text = "Incorrect";
                Score--;
                scoreLabel.text = "Score:\n" + Score;
            //} else {
           //     if (GameOver != true)
            //        GameOverScreen ();
            //}

        }
    }
    void ZenModeSave () {
        timeLeft = 0f;
        pauseScreen.SetActive (true);
        LevelScore.levelUnlock[1] = 1;
        LevelScore.levelStar[0] = 3;
        LevelScore.highScore = 9999;
        LevelScore.timeMode = true;
        saveSystem.SaveData (LevelScore);
    }
    void ClassicMode () {
        timeForward += Time.deltaTime;
        countDownLabel.text = "";
        drawLabel.text = randomGesture.Name;
        timeLabel.text = "Time Left: " + Mathf.RoundToInt (timeForward);
        DrawMode ();
    }
    void ClassicModeCheck () {

    }
    void ClassicModeSave () {

    }
    void TaskOnClick () {
        recognized = true;
        Gesture candidate = new Gesture (points.ToArray ());
        gestureResult = PointCloudRecognizer.Classify (randomGesture.Name, candidate, trainingSet.ToArray ());
        if (isZenMode == true)
            ZenModeCheck ();
        else
            ClassicModeCheck ();

        message = "You have written" + gestureResult.GestureClass + " " + gestureResult.Score;
        ClearBoard ();
    }

    void ClearBoard () {
        points.Clear ();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer) {
            lineRenderer.positionCount = 0;
            Destroy (lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear ();
    }

    void GameOverScreen () {
        GameOver = true;
        if (isZenMode == true)
            ZenModeSave ();
        else
            ClassicModeSave ();
    }

    private void DrawMode () {
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
            if (Input.touchCount > 0) {
                verifyLabel.text = "";
                virtualKeyPosition = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
            }
        } else {
            if (Input.GetMouseButton (0)) {
                verifyLabel.text = "";
                virtualKeyPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        if (drawArea.Contains (virtualKeyPosition)) {

            if (Input.GetMouseButtonDown (0)) {

                if (recognized) {
                    recognized = false;
                    strokeId = -1;
                    ClearBoard ();
                }

                ++strokeId;

                Transform tmpGesture = Instantiate (gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer> ();

                gestureLinesRenderer.Add (currentGestureLineRenderer);

                vertexCount = 0;
            }

            if (Input.GetMouseButton (0)) {
                points.Add (new Point (virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

                // currentGestureLineRenderer.SetVertexCount (++vertexCount);
                currentGestureLineRenderer.positionCount = ++vertexCount;
                currentGestureLineRenderer.SetPosition (vertexCount - 1, Camera.main.ScreenToWorldPoint (new Vector3 (virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
        }
    }
}