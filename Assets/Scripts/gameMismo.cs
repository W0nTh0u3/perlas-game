using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PDollarGestureRecognizer;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameMismo : MonoBehaviour {
    
    public Animator starController;
    public Animator animationRedGreen;
    public Image hintImage;
    public Image drawPanel;
    public GameObject hintButton;
    public Button recognizeButton;
    public Transform gestureOnScreenPrefab;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI levelDescText;
    public Text drawLabel;
    public Text scoreLabel;
    public Text timeLabel;
    public Text countDownLabel;
    public GameObject gameOverScreen;
    public GameObject nextLevelBtn;

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

    private Gesture thisGesture;
    private Gesture randomGesture;
    private Gesture[] selections;
    private string[] mustLoadName;
    private string levelDescName;
    private Sprite[] hintImagesList;
    private Sprite thisHint;
    private Sprite blankSquare;
    private Data LevelScore = new Data ();
    private dataC loadedData;
    private int x = 0;
    private int Score = 0;
    private bool isHintShown = false;
    //GUI
    private bool recognized;
    private bool GameOver = false;

    //private string newGestureName = "";
    private float timeLeft = 21f;
    private float countDownTime = 4.4f;
    private float timeForward = 0f;
    private float hintTimer = 0f;
    private bool isZenMode;
    private int numLevelClicked = 0;
    private Result gestureResult;
    void Start () {
        Debug.Log ("Save Data Path: " + Application.persistentDataPath.ToString ());
        animationRedGreen.Play ("DefaultDraw");
        isZenMode = BoolPrefs.GetBool ("isTimeAttack");
        if (isZenMode == false) {
            numLevelClicked = PlayerPrefs.GetInt ("levelToLoad");
        }
        loadedData = saveSystem.LoadData ();
        Button btn = recognizeButton.GetComponent<Button> ();
        btn.onClick.AddListener (TaskOnClick);
        platform = Application.platform;
        Debug.Log ("Is Zen Mode? " + isZenMode.ToString ());
        Debug.Log ("This Is Level: " + numLevelClicked.ToString ());
        //Load pre-made gestures
        LoadHintImagesResources();
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset> ("GestureSet/newSetBaybayin/");
        foreach (TextAsset gestureXml in gesturesXml) {
            trainingSet.Add (GestureIO.ReadGestureFromXML (gestureXml.text));
        }
        selections = trainingSet.ToArray ();
        hintImage.sprite = blankSquare;
        if (isZenMode == true) {
            nextLevelBtn.SetActive(false);
            scoreLabel.text = "Score:\n" + Score;
            levelDescText.text = "";
            Shuffler ();
        } else {
            LoadLevelLetters ();
            FindLettersClassic ();
            scoreLabel.text = "";
            levelDescText.text = "Level " + numLevelClicked + ":\n" + "\"" + levelDescName + "\"";
        }
        drawLabel.text = "";
        countDownLabel.text = "";
    }

    public void ShowHintImage()
    {
        if (isHintShown == false)
        {
            isHintShown = true;
            hintImage.sprite = thisHint;
        }
        else
        {
            isHintShown = false;
            hintImage.sprite = blankSquare;
        }
    }

    void LoadHintImagesResources()
    {
        UnityEngine.Object[] baybayinLettersObj = Resources.LoadAll("baybayinReference/", typeof(Sprite));
        hintImagesList = new Sprite[baybayinLettersObj.Length];
        for (int d = 0; d < baybayinLettersObj.Length; d++)
            hintImagesList[d] = (Sprite)baybayinLettersObj[d];
        blankSquare = hintImagesList.SingleOrDefault(cd => cd.name == "blankSquare");
        //Debug.Log("Hi");

    }

    void Shuffler () {
        randomGesture = selections[UnityEngine.Random.Range (0, selections.Length)];
        thisHint = hintImagesList.SingleOrDefault(cd => cd.name == randomGesture.Name.Replace("/",""));
        //hintImage.sprite = 
    }
    void FindLettersClassic () {
        thisGesture = selections.SingleOrDefault (cd => cd.Name == mustLoadName[x]);
        thisHint = hintImagesList.SingleOrDefault(cd => cd.name == thisGesture.Name.Replace("/", ""));
    }
    void LoadLevelLetters () {
        TextAsset ta = Resources.Load ("baybayinLevels") as TextAsset;
        JSONObject levelJson = (JSONObject) JSON.Parse (ta.text);
        mustLoadName = new string[levelJson["baybayin"][numLevelClicked - 1]["letters"].Count];
        levelDescName = levelJson["baybayin"][numLevelClicked - 1]["levelName"];
        //baybayinLetters = levelJson["baybayin"][1]["letters"].AsArray;
        for (int i = 0; i < levelJson["baybayin"][numLevelClicked - 1]["letters"].Count; i++) {
            mustLoadName[i] = levelJson["baybayin"][numLevelClicked - 1]["letters"][i];
        }

    }

    void Update () {
        if (countDownTime <= 0) {
            if (isZenMode == true)
                ZenMode ();
            else
                ClassicMode ();
            hintTimer += Time.deltaTime;

        } else {
            countDownTime -= Time.deltaTime;
            if (Mathf.RoundToInt (countDownTime) == 0)
                countDownLabel.text = "Draw!";
            else
                countDownLabel.text = Mathf.RoundToInt (countDownTime).ToString ();
        }
        if (hintTimer >= 5)
            hintButton.SetActive(true);
        else
            hintButton.SetActive(false);
    }
    void ZenMode () {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0) {
            countDownLabel.text = "";
            drawLabel.text = randomGesture.Name.Replace("/"," / ");
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
            animationRedGreen.Play ("greenDraw", -1, 0f);
            //verifyLabel.text = "Correct: " + percentScore.ToString () + " %";
            if (isHintShown == true)
                ShowHintImage();
            //verifyLabel.text = "Correct";
            Handheld.Vibrate ();
            Score++;
            scoreLabel.text = "Score:\n" + Score;
            timeLeft += 5f;
            Shuffler ();
            hintTimer = 0f;
        } else {
            if (timeLeft > 0) {
                animationRedGreen.Play ("redDraw", -1, 0f);
                //verifyLabel.text = "Incorrect";
                Handheld.Vibrate ();
                timeLeft -= 1f;
                scoreLabel.text = "Score:\n" + Score;
            } else {
                if (GameOver != true) {
                    animationRedGreen.Play ("redDraw", -1, 0f);
                    Handheld.Vibrate ();
                    GameOverScreen ();
                }

            }

        }
    }
    void ZenModeSave () {
        timeLeft = 0f;
        gameOverScreen.SetActive (true);
        starController.Play ("defaultPlank", -1, -0.5f);
        if (Score > loadedData.highScore)
        {
            LevelScore.highScore = Score;
            gameOverScoreText.text = "Final Score: " + Score + "\nNew! High Score";
        }
        else
        {
            LevelScore.highScore = loadedData.highScore;
            gameOverScoreText.text = "Final Score: " + Score + "\nHigh Score: " + LevelScore.highScore;
        }
        LevelScore.levelUnlock = loadedData.levelUnlock;
        LevelScore.levelStar = loadedData.levelStar;
        LevelScore.timeMode = true;
        saveSystem.SaveData (LevelScore);
    }
    void ClassicMode () {
        if (GameOver != true) {
            timeForward += Time.deltaTime;
            countDownLabel.text = "";
            drawLabel.text = thisGesture.Name.Replace("/", " / ");
            timeLabel.text = Mathf.RoundToInt (timeForward).ToString ();
            DrawMode ();
        }
    }
    void ClassicModeCheck () {
        if (gestureResult.Score > 0.9f) {
            float percentScore = (gestureResult.Score) * 100;
            animationRedGreen.Play ("greenDraw", -1, 0f);
            //verifyLabel.text = "Correct: " + percentScore.ToString () + " %";
            if (isHintShown == true)
                ShowHintImage();
            //verifyLabel.text = "Correct";
            Handheld.Vibrate ();
            x++;
            if (x >= mustLoadName.Length)
                GameOverScreen ();
            else
                FindLettersClassic ();
            hintTimer = 0f;
        } else {
            animationRedGreen.Play ("redDraw", -1, 0f);
            //verifyLabel.text = "Incorrect";
            Handheld.Vibrate ();
        }
    }
    void ClassicModeSave () {
        gameOverScreen.SetActive (true);
        gameOverScoreText.text = "Time Finished : " + Mathf.RoundToInt (timeForward).ToString () + " s";
        LoadClassicSave();
        int starsGot = StarScoring();
        if (loadedData == null)
            LevelScore.levelStar[numLevelClicked - 1] = starsGot;
        else
        {
            if (loadedData.levelStar[numLevelClicked - 1] < starsGot)
                LevelScore.levelStar[numLevelClicked - 1] = starsGot;
        }
        if (numLevelClicked < 10)
        {
            if (starsGot > 1)
            {
                LevelScore.levelUnlock[numLevelClicked] = 1;
                nextLevelBtn.SetActive(true);
            }
            else
            {
                if (LevelScore.levelUnlock[numLevelClicked] != 1)
                    LevelScore.levelUnlock[numLevelClicked] = 0;
                nextLevelBtn.SetActive(false);
            }   
        }
        else
            nextLevelBtn.SetActive(false);
        for(int icool = 0; icool < LevelScore.levelStar.Length; icool++)
        {
            if (LevelScore.levelStar[icool] == 3)
                LevelScore.timeMode = true;
            else
            {
                LevelScore.timeMode = false;
                break;
            }
        }
        saveSystem.SaveData(LevelScore);
    }

    void LoadClassicSave()
    {
        if (loadedData == null)
        {
            LevelScore.levelUnlock[0] = 1;
        }
        else
        {
            LevelScore.levelUnlock = loadedData.levelUnlock;
            LevelScore.levelStar = loadedData.levelStar;
            LevelScore.highScore = loadedData.highScore;
            LevelScore.timeMode = loadedData.timeMode;
        }
    }
    int StarScoring()
    {
        int stars;
        if (timeForward > (mustLoadName.Length * 10))
        {
            starController.Play("1star", -1, -0.5f);
            stars = 1;
        }
        else if (timeForward > (mustLoadName.Length * 5))
        {
            starController.Play("2star", -1, -0.5f);
            stars = 2;
        }
        else
        {
            starController.Play("3star", -1, -0.5f);
            stars = 3;
        }
        return stars;
        
    }
    void TaskOnClick () {
        recognized = true;
        Gesture candidate = new Gesture (points.ToArray ());
        if (isZenMode == true) {
            gestureResult = PointCloudRecognizer.Classify (randomGesture.Name, candidate, trainingSet.ToArray ());
            Debug.Log(gestureResult.GestureClass + " : " + gestureResult.Score);
            ZenModeCheck ();
        } else {
            gestureResult = PointCloudRecognizer.Classify (thisGesture.Name, candidate, trainingSet.ToArray ());
            Debug.Log(gestureResult.GestureClass + " : " + gestureResult.Score);
            ClassicModeCheck ();
        }
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

    public void UndoBoard()
    {
        if(strokeId >= 0)
        {
            points.RemoveAll(e => e.StrokeID == strokeId);
            Destroy(gestureLinesRenderer.Last().gameObject);
            gestureLinesRenderer.RemoveAt(gestureLinesRenderer.Count - 1);
            --strokeId;
            //Destroy(gestureLinesRenderer.gameObject);
        }
        
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
                //verifyLabel.text = "";
                virtualKeyPosition = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
            }
        } else {
            if (Input.GetMouseButton (0)) {
                //verifyLabel.text = "";
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