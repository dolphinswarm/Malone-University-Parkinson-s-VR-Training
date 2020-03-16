using UnityEngine;
using UnityEngine.UI;

/* NOTES:

	General approach is to have a small number of info boards positioned in stable, key places
	and let it's content be filled dynamically by different events.

	InfoBoard can display either questions or instructions.
	It can play an audio clip on demand (e.g., give instructions, beep at wrong answers, etc.)
	It can be cleared / emptied when finished.

	Individual questions / instructions should manage their own content and timing,
	and determine whether their answers are right / wrong.
	The InfoBoard should not be used to manage event flow... it is for display only.
		- Except.... it could perhaps include a "go back" button to replay the last item.


// */


/// <summary>
/// The user interface for the info board.
/// </summary>
public class InfoBoardUI : MonoBehaviour {
    // ======================================================== Variables
    [Header("Game Manager")]
    public GameSettings gameManager;
    public bool useGameMgrDefaults = true;

    [Header("UI Colors")]
    public float highlightPct = 0.1f;
    public Color selectedColor = Color.green;
	public Color normalColor = Color.gray;
	public Color rightColor = Color.green;
	public Color wrongColor = Color.red;

    [Header("Audio")]
    public AudioClip correctSFX;
	public AudioClip wrongSFX;

    [Header("Question Display Managers")]
    public QuestionDisplayManager questionDisplay_4;    // change the type to QuestionDisplayManager
    public QuestionDisplayManager questionDisplay_5;
    public QuestionDisplayManager questionDisplay_6;
    public QuestionDisplayManager questionDisplay_8;
    public QuestionDisplayManager questionDisplay_10;
    public QuestionDisplayManager questionDisplay_14;
    public GameObject infoDisplay;      // change type?
    QuestionDisplayManager questionDisplay; // generic hook, gets overwritten later
    public GameObject controllerImage;

    // ======================================================== Methods
    /// <summary>
    /// Used for initialization.
    /// </summary>
    void Start() {
        // Finds game manager, if not present
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager

        // If controller image not found, add it
        if (controllerImage == null)
        {
            controllerImage = GameObject.Find("Oculus Controllers Image");
            controllerImage.SetActive(false);
        }


        // Adds Question-4 as a default question display
        if (questionDisplay == null)
            questionDisplay = transform.Find("Question-4").GetComponent<QuestionDisplayManager>();

        // If we want to use game manager defaults, set them
        if (useGameMgrDefaults) {
            highlightPct = gameManager.highlightPct;
            selectedColor = gameManager.selectedColor;
            normalColor = gameManager.normalColor;
            rightColor = gameManager.rightColor;
            wrongColor = gameManager.wrongColor;
            correctSFX = gameManager.correctSFX;
            wrongSFX = gameManager.wrongSFX;
        }
	}

    /// <summary>
    /// Called by GameObjects with an InfoObject script, used to display instructions / messages.
    /// </summary>
    /// <param name="newInfo">Information</param>
	public void SetInfo(InfoObject newInfo) {
        // toggle visibility
        if (questionDisplay != null) { questionDisplay.gameObject.SetActive(false); }
        infoDisplay.SetActive(true);

		// set the text of the info display
		infoDisplay.GetComponent<Text>().text = newInfo.infoText;
		
        // set skippable settings as applicable
        if (newInfo.clickToSkip) {
            SkipHandler mySkipper = infoDisplay.GetComponent<SkipHandler>();
            mySkipper.clickToSkip = newInfo.clickToSkip;
            mySkipper.dontSkipFirstSec = newInfo.dontSkipFirstSec;
            mySkipper.sourceEvent = newInfo.gameObject;
            mySkipper.Go();
            
        }
     
        // play voiceover if it exists
        if (newInfo.voiceOver != null) {
			GetComponent<AudioSource>().PlayOneShot(newInfo.voiceOver);
		}
	}

   
    /// <summary>
    /// Shows instructions for a step
    /// </summary>
    /// <param name="newText"></param>
    public void ShowInstructions(string newText) {
        // toggle visibility
        if (questionDisplay != null) { questionDisplay.gameObject.SetActive(false); }
        infoDisplay.SetActive(true);

        // set the text of the info display
        infoDisplay.GetComponent<Text>().text = newText;

        // disable skip function with simple instructions...
        infoDisplay.GetComponent<SkipHandler>().clickToSkip = false;
    }


    /// <summary>
    /// Called by GameObjects with QuestionEvent script, used to display a question.
    /// </summary>
    /// <param name="myQuestion">Question</param>
	public void SetQuestion(QuestionEvent myQuestion) {
		// toggle visibility
		infoDisplay.SetActive(false);

        // determine which QuestionEvent to use based on number of answers
        // ....and assign it to generic questionDisplay object
        if (myQuestion.answers.Length <= 4) {
            questionDisplay = questionDisplay_4;
        }
        else if (myQuestion.answers.Length == 5) {
            questionDisplay = questionDisplay_5;
        }
        else if (myQuestion.answers.Length <= 6) {
            questionDisplay = questionDisplay_6;
        }
        else if (myQuestion.answers.Length <= 8) {
            questionDisplay = questionDisplay_8;
        }
        else if (myQuestion.answers.Length <= 10) {
            questionDisplay = questionDisplay_10;
        }
        else if (myQuestion.answers.Length <= 14)
        {
            questionDisplay = questionDisplay_14;
        }
        else {
            Debug.Log("Too many answers in question: " + myQuestion.gameObject.name);
            infoDisplay.SetActive(true);
            infoDisplay.GetComponent<Text>().text = "Too many answers in question: " + myQuestion.gameObject.name;
        }

        // toggle visibility of question object
        questionDisplay.gameObject.SetActive(true);
        // set question text
        questionDisplay.GetComponent<Text>().text = myQuestion.questionText;
        // store reference to original object
        questionDisplay.myQuestion = myQuestion;

        // toggle set-all functionality
        questionDisplay.mySelectAll.SetActive(myQuestion.showAllOfTheAbove);

        // set answer texts
        questionDisplay.SetAnswers(myQuestion.answers);

        if (myQuestion.voiceOver != null)
        {
            GetComponent<AudioSource>().PlayOneShot(myQuestion.voiceOver);
        }

        // questionDisplay.SetSubmitButton();
        questionDisplay.mySubmitButton.SetActive(true);
    }

    /// <summary>
    /// Called externally, used to hide all contents / empty the InfoBoard.
    /// </summary>
    public void Reset() {
		// easiest way to clear the info board is to hide the instructions / questions objects
		questionDisplay.gameObject.SetActive(false);
		infoDisplay.SetActive(false);

        // Stop playing any playing audio
        if (GetComponent<AudioSource>().isPlaying) { GetComponent<AudioSource>().Stop(); }
	}

    /// <summary>
    /// Called externally, used to pass data to ReportCardManager.
    /// </summary>
    /// <param name="dataToLog"></param>
    public void Log(string dataToLog) {
        gameManager.reportCardManager.writeLine(dataToLog);
    }

}
