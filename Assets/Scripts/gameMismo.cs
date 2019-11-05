using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;

public class gameMismo : MonoBehaviour {
	public Button recognizeButton;
	public Transform gestureOnScreenPrefab;
	public Text drawLabel;
	public Text verifyLabel;
	public Text scoreLabel;
	public Text timeLabel;
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
	
	//private string newGestureName = "";
	private float timeLeft = 10f;
	void Start () {
		Button btn = recognizeButton.GetComponent<Button> ();
		btn.onClick.AddListener (taskOnClick);
		platform = Application.platform;
		//drawArea = new Rect(0, 0, Screen.width, Screen.height / 2);

		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset> ("GestureSet/baybayin/");
		foreach (TextAsset gestureXml in gesturesXml) {
			trainingSet.Add (GestureIO.ReadGestureFromXML (gestureXml.text));
		}
		selections = trainingSet.ToArray ();
		shuffler ();
		Debug.Log (randomGesture.Name);
		scoreLabel.text = "Score: " + Score;
		// Gesture randomGesture = 
		// foreach(Gesture listings in trainingSet.ToArray()){
		// 	Debug.Log(listings.Name);
		// }
		// //Load user custom gestures
		// string[] filePaths = Directory.GetFiles (Application.persistentDataPath, "*.xml");
		// foreach (string filePath in filePaths)
		// 	trainingSet.Add (GestureIO.ReadGestureFromFile (filePath));
	}

	void shuffler () {
		randomGesture = selections[UnityEngine.Random.Range (0, selections.Length)];
		drawLabel.text = randomGesture.Name;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft >= 0) {
			timeLabel.text = "Time Left: " + Mathf.RoundToInt(timeLeft);
			if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
				if (Input.touchCount > 0) {
					virtualKeyPosition = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
				}
			} else {
				if (Input.GetMouseButton (0)) {
					virtualKeyPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
				}
			}

			if (drawArea.Contains (virtualKeyPosition)) {

				if (Input.GetMouseButtonDown (0)) {

					if (recognized) {
						recognized = false;
						strokeId = -1;
						clearBoard ();
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
		else {
			gameOverScreen();
		}
	}

	void taskOnClick () {
		recognized = true;
		Gesture candidate = new Gesture (points.ToArray ());
		Result gestureResult = PointCloudRecognizer.Classify (randomGesture.Name, candidate, trainingSet.ToArray ());
		if (gestureResult.Score > 0.9f) {
			float percentScore = (gestureResult.Score) * 100;
			verifyLabel.text = "Correct: " + percentScore.ToString () + " %";
			Score++;
			scoreLabel.text = "Score: " + Score;
			timeLeft += 10f;
			shuffler ();
		} else {
			verifyLabel.text = "Incorrect";
			Score--;
			scoreLabel.text = "Score: " + Score;
		}

		message = "You have written" + gestureResult.GestureClass + " " + gestureResult.Score;
		clearBoard ();
	}

	void clearBoard () {
		points.Clear ();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

			// lineRenderer.SetVertexCount (0);
			lineRenderer.positionCount = 0;
			Destroy (lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear ();
	}

	void gameOverScreen(){
		pauseScreen.SetActive(true);
	}

	// void OnGUI () {
	// 	//GUI.Box (boxArea, "Draw Box");

	// 	//GUI.Label (new Rect (10, 0, 500, 50), message);

	// 	// if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize")) {

	// 	//GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
	// 	//newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

	// 	// if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "") {

	// 	// 	string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());

	// 	// 	#if !UNITY_WEBPLAYER
	// 	// 		GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
	// 	// 	#endif

	// 	// 	trainingSet.Add(new Gesture(points.ToArray(), newGestureName));

	// 	// 	newGestureName = "";
	// 	// }
	// }
}