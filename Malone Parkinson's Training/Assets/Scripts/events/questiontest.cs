using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class questiontest : MonoBehaviour {

	// scene to load
	public String sceneToLoad;

	public AudioClip questionAudio, incorrectAnswer, correctAnswer;

	private GameObject player;
	private AudioSource playerSource;
	private bool hasPlayedAudio;

	// Public variable for statemanagement by the "QuestionSeriesManager" code.
	public bool completed;
	public bool job1, job2, job3;

	// Variables for managing question/answer stuff.
	public string questionText;
	public string incorrectAnswerExplanation = "You were wrong.";
	public bool multipleAnswers;

	private Color shouldaPicked = new Color32(72, 170, 72, 255);
	private Color selectedColor = new Color32(10,200,200,255);
	private Color originalColor;
	private List<QuestionEventTest> answers;

	// Declaration of variables used for raycasting out of the camera.
	private Camera mCamera; // mCamera is the Main Camera, thus the M.
	private Ray cameRay;
	private RaycastHit hit;

	// Declaration of variables used for "report card" stuff.
	public bool correct;
	public string reportCardExportString;
	public string computerReportCardExportString;

	// Used to store the Game Manager.
	private GameObject gameManager;
	private ReportCardManager reportCard;

    private bool hasBeenClicked = false;

    // Use this for initialization
    void Start () {

		// Assigns gameManager to a gameobject named "Game Manager".
		gameManager = GameObject.Find("Game Manager");
		reportCard = gameManager.GetComponent<ReportCardManager>();


        if (GameObject.Find("First Person Controller") != null)
        {
            player = GameObject.Find("First Person Controller");
        }
        else if (GameObject.Find("OVRPlayerController") != null)
        {
            player = GameObject.Find("OVRPlayerController");
        }
        playerSource = player.GetComponent<AudioSource> ();

		if(this.questionAudio != null){
			playerSource.clip = questionAudio;
		}


		// The reportCardExportString variable is used to create a human-readable string that 
		// can easily be written to a .txt file. The format of this line is as follows:
		// "QuestionGameObjectName: ActualQuestionText - CORRECT/INCORRECT". These values
		// are set here, and then also in the questionSeriesManager.
		reportCardExportString += (this.gameObject.name + ": " + questionText);

		this.GetComponent<Text> ().text = questionText;

		// Grabs the camera in the scene tagged as "MainCamera" and is used as the main camera
		// for raycasting and such. Logs an error if the Main Camera isn't found.
		mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		if (mCamera == null) {
						Debug.Log ("Main camera not found.");
				} else {
			//Debug.Log ("Main Camera Found");
				}

		// Creates a ray shooting straight out of the center of the viewpoint of the camera.
		cameRay = mCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));

		// This section grabs all the answers from the child objects and adds them to an array
		// of possible answers. If answers have text that is left empty, the answer is removed
		// from the array. In this test, correct answers are highlighted green and incorrect
		// answers are highlighted red. In addition, all correctly entered question text is
		// logged to the console for testing purposes.

		bool notEmpty = false; // Tracks whether or not the answers array is empty.

		// Here, we instantiate a list for the sole purpose of checking for empty strings
		// inside of answers.
		List<QuestionEventTest> tempAnswers = new List<QuestionEventTest>();
		answers = new List<QuestionEventTest>();

		// Answers is populated with the QuestionEventTest components of each of its children
		// which have a QuestionEventtest component.
		foreach(QuestionEventTest quest in this.GetComponentsInChildren<QuestionEventTest>()){
			originalColor = quest.gameObject.GetComponent<Text>().color;
			answers.Add (quest);
		}
		foreach(QuestionEventTest quest in answers){
			tempAnswers.Add(quest);
		}

		// If an answer has no text for its answer, the answer is removed from the array.
		foreach(QuestionEventTest ans in tempAnswers){
			if(ans.answerText == string.Empty){
				ans.gameObject.SetActive(false);
				answers.Remove(ans);
				Debug.Log (gameObject.name.ToString () + " - Removing empty answer - " + ans.gameObject.name.ToString() );
			}else{
				notEmpty = true;
			}
		}

		// Now we check if the question is a multiple-answer or "list-style" question. If
		// more than one answer is labeled as correct, then it is a list-style question.
		bool answer1 = false;
		foreach(QuestionEventTest ans in answers){
			if(ans.correct == true){
				if(answer1){
					multipleAnswers = true;
				}else{
					answer1 = true;
				}
			}
		}
	}



	// Method to load next level... can be invoked after a delay for audio
	void LoadNextLevel() {
		Debug.Log ("****Finishing orientation -- Loading next level****");
		SceneManager.LoadScene(sceneToLoad);
	}

	// Update is called once per frame
	void Update () {

		if (!hasPlayedAudio) {
			playerSource.clip = questionAudio;
			playerSource.Play ();
			hasPlayedAudio = true;
		}

		// This variable is used to track whether or not the raycast from the camera hit
		// anything. This information is used for 'hovering' purposes.
		bool noHit = true;

		// This draws a new raycast every frame.
		cameRay = mCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(mCamera.transform.position, mCamera.transform.forward * 10, Color.green);
	
		// Iterates through every answer object in the answers array.
		foreach(QuestionEventTest ans in answers){
			// Checks if the raycast hit an answer, and only runs if the question has not
			// been completed.
			if(Physics.Raycast(cameRay, out hit) && !completed){
				//Debug.Log (hit.collider.gameObject.name);
				// If the raycast hits a possible answer, the answer is "hovered" and noHit
				// is set to false. If the Fire1 (LMB at time of writing) is pressed, the
				// answer is selected.
				if(hit.collider.gameObject == ans.gameObject){
					//Debug.Log ("HIT AT " + answers.IndexOf(ans));
					hover (ans.gameObject);
					noHit = false;
                    if (Input.GetButtonDown("Fire1") ) {
                    //if (!hasBeenClicked && (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) ){
                        hasBeenClicked = true;
                        // If the clicked answer is tagged as a selector, answerValidation
                        // is run.
                        if (ans.selector){
							answerValidation();
						}else{
							select(ans.gameObject);
						}
					}
				}
				// Checks if the raycast hits the gameObject attached to this code.
			}
		}
		// If the raycast didn't hit anything, then all answers are "unhovered".
		if(noHit){
			unHover();
		}
	}

	// Answer validation is run when the "done" button is pressed. It checks all selected answers
	// against the answers list. If the selected answer(s) is/are correct, they are turned 
	// bright green. If the selected answer(s) is/are incorrect, they are turned red, and the correct
	// answers are highlighted in a color to indicate they should have been selected (dark green, at
	// time of writing). Additionally, if a correct answer is indicated, the boolean "correct" is set
	// to correct, and any incorrect answers set the boolean "correct" to false.
	void answerValidation(){

        if (GameObject.Find("First Person Controller") != null)
        {
            player = GameObject.Find("First Person Controller");
        }
        else if (GameObject.Find("OVRPlayerController") != null)
        {
            player = GameObject.Find("OVRPlayerController");
        }
        playerSource = player.GetComponent<AudioSource> ();

		if (this.gameObject.name != "Character Select") {
			correct = true;
			foreach (QuestionEventTest ans in answers) {
				if (ans.selected && !ans.correct) {
					ans.gameObject.GetComponent<Text> ().color = Color.red;
					computerReportCardExportString += ("The question you got wrong was: " + questionText + "#");
					computerReportCardExportString += Environment.NewLine;
					computerReportCardExportString += ("The answer you gave was: " + ans.answerText + "#"); 
					computerReportCardExportString += Environment.NewLine;
					computerReportCardExportString += incorrectAnswerExplanation;
					computerReportCardExportString += Environment.NewLine;
					computerReportCardExportString += Environment.NewLine;
					if(multipleAnswers){
						computerReportCardExportString += "The correct answers are: " + Environment.NewLine;
					}else{
						computerReportCardExportString += "The correct answer is: ";
					}

					foreach (QuestionEventTest ans2 in answers) {
						if (ans2.correct) {
							computerReportCardExportString += ans2.answerText;
							if(multipleAnswers){
								computerReportCardExportString += Environment.NewLine;
							}
						}
					}
					computerReportCardExportString += "##";
					correct = false;
				} else if (ans.selected && ans.correct) {
					//Debug.Log ("Correct answer selected");
					ans.gameObject.GetComponent<Text> ().color = Color.green;
				} else if (ans.correct && multipleAnswers) {
					ans.gameObject.GetComponent<Text> ().color = shouldaPicked;
					correct = false;
				} else if (ans.correct) {
					ans.gameObject.GetComponent<Text> ().color = shouldaPicked;
					correct = false;
				}
			}
			if (correct) {
				playerSource.clip =  correctAnswer; // (AudioClip)Resources.Load ("CORRECT");
			} else {
					playerSource.clip = incorrectAnswer;
			}
			playerSource.Play ();
			selectNone ();
			completed = true;

		} else {
			foreach (QuestionEventTest ans in answers) {
				if(ans.selected) {
					//Debug.Log("---Answer Selected---");	Debug.Log(ans);	Debug.Log("Text: " + ans.answerText); Debug.Log("Name: " + ans.name.ToString()); Debug.Log("Index: " + answers.IndexOf(ans).ToString());
					reportCard.writeLine("Playing " + sceneToLoad.ToString() + " as " + ans.answerText + "\n");
					GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job1 = false;
					GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job2 = false;
					GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job3 = false;
					if (ans.name == "Answer 1") {   //(answers.IndexOf (ans) == 0) {
						GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job1 = true;
						Debug.Log("Job 1 Selected (MT)");
					} else if (ans.name == "Answer 2") {   //(answers.IndexOf (ans) == 1) {
						GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job2 = true;
						Debug.Log("Job 2 Selected (RT)");
					} else if (ans.name == "Answer 3") {   //(answers.IndexOf (ans) == 2) {
						GameObject.Find ("Character Select Manager").GetComponent<characterSelectManager> ().job3 = true;
						Debug.Log("Job 3 Selected (Nurse)");
					}

					// start fade to black
					GameObject.Find("FadeToBlack").SendMessage("EndScene");

					// load sceneToLoad with ~2s delay
					Invoke("LoadNextLevel", 2.1f);

					// play audio feedback
					playerSource.clip =  correctAnswer; // (AudioClip)Resources.Load ("CORRECT");
					playerSource.Play ();

				}
			}
		}
	}

	// Hover is passed a gameobject, which has the FontStyle of its text component changed to
	// bold and italics. Every other object has the FontStyle of its text component set to normal.
	void hover(GameObject que){
		foreach(QuestionEventTest que2 in answers){
			que2.gameObject.GetComponent<Text>().fontStyle = FontStyle.Normal;
		}
		que.gameObject.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
	}

	// UnHover changes all the FontStyle of all answers in the answer array, as well as the
	// FontStyle of this object itself, to normal.
	void unHover(){
		foreach (QuestionEventTest que in answers){
			que.gameObject.GetComponent<Text>().fontStyle = FontStyle.Normal;
		}
	}

	// SelectNone changes the "selected" value of all answers in the answers list to false.
	void selectNone(){
		foreach(QuestionEventTest qObj in answers){
			qObj.selected = false;
		}
	}
	
	// This method is used to select one answer within the "answers" array and change its
	// selected value to true, and if the question is not list-style, then it changes all 
	// other selected values to false.
	void select(GameObject obj){
			if (obj.GetComponent<QuestionEventTest> ().selected == false) {
				if (!multipleAnswers) {
					foreach (QuestionEventTest qObj in answers) {
						qObj.selected = false;
						qObj.gameObject.GetComponent<Text> ().color = originalColor;
					}
				}
				obj.GetComponent<QuestionEventTest> ().selected = true;
				obj.gameObject.GetComponent<Text> ().color = selectedColor;
		} else {
			obj.GetComponent<QuestionEventTest>().selected = false;
			obj.GetComponent<Text>().color = originalColor;
		}
	}
}