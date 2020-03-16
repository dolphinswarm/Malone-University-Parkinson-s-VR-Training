using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// The Answer class, used by a few various related classes.
/// </summary>
[Serializable]
public class Answer {
	public string answerText	= "Answer Choice";
	public bool isCorrect		= false;

	public Answer (string text, bool correct) {
		answerText = text;
		isCorrect = correct;
	}

}

/// <summary>
/// A question event, which displays a question for the participant to answer on the board.
/// </summary>
public class QuestionEvent : InfoBoardEvent {
    // ======================================================== Variables
    [Header("Info Board / UI")]
    public string questionText;			                    // Text of question
	public string redirectionText;                          // Text to display / Log if wrong
    public bool showAllOfTheAbove = false;                  // Display "All of the Above" as an option and use for toggling

    [Header("Audio")]
    public AudioClip redirectionAudio;                      // Voiceover to play if answered incorrectly
	public AudioClip correctAudio;                          // Audio clip to play if correct.... defaults to InfoBoard's SFX

    [Header("Question Properties")]
    // consider using an enum for single answer, multi-answer, open-ended (single)
    public bool multipleAnswers = false;                    // Allow multiple answers?
    public bool openEnded = false;                          // No correct answer, need your opinion
    public bool submitOnSelection = false;                  // Submit on selection of an answer?
	bool completedCorrectly;                                // Note whether it was answered correctly or not
    public bool hasBeenSubmitted = false;
    public Answer[] answers = new Answer[4];                // Array of answers (each contains string "answerText", bool isCorrect)

    // ======================================================== Methods
    /// <summary>
    /// Initializes this delivery event.
    /// </summary>
    protected override void Initialize()
    {
        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// Starts this event.
    /// </summary>
    /// <param name="prevEventNum">The previous event number</param>
    public override void Go(int prevEventNum)
    {
        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Sets the question in the Question Object
        infoBoard.SetQuestion(gameObject.GetComponent<QuestionEvent>());    // pass along reference to this script

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Question Event***: Event #" + myEventNum);
    }

    /// <summary>
    /// Marks an answer as sumbmitted, and prosses a list of open-ended answers.
    /// </summary>
    /// <param name="thingsSelected">A list of selected answers</param>
    public void ReportAnswer(List<AnswerManager> thingsSelected) {
        // we've been notified that an answer was given, as a selected list
        // These will be special cases... let's pass them on.
        GetComponent<OpenEndedQuestion>().ProcessAnswers(thingsSelected);

        // Submit this answer as "correct" and move on (can't get open-ended questions wrong)
        SubmitAnswer(true);
    }

    /// <summary>
    /// Handles whether a question has been answered correctly / incorrectly.
    /// </summary>
    /// <param name="answerIsCorrect"></param>
    public void SubmitAnswer(bool answerIsCorrect) {
		// save right/wrong state
		completedCorrectly = answerIsCorrect;

        // Set this question to answered
        hasBeenSubmitted = true;

        // empty string for writing to log file
        string outString = "";

        // Get the report card, if used
        ReportCardManager reportCard = GameObject.Find("Game Manager").GetComponent<ReportCardManager>();

        // respond appropriately
        if (answerIsCorrect) {
			// play Correct! sound effect
			if(correctAudio != null) {
				infoBoard.GetComponent<AudioSource>().PlayOneShot(correctAudio);
                Invoke("Finished", correctAudio.length + delayBeforeAdvance);
                
            }
			else if (infoBoard.correctSFX != null) {
				infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
                Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
            }
            else
            {
                Invoke("Finished", delayBeforeAdvance);
            }

            // write appropriate entry to the logfile here
            outString += myEventNum.ToString() + "\t";
            outString += questionText + "\t";
            outString += "CORRECT";
            outString += "#";

            // INCREMENT SCORE HERE????
            // If report card is used, write a line
            if (reportCard != null && reportCard.isActiveAndEnabled)
            {
                reportCard.writeLine(outString);
            }

        }
        else {
            // play Wrong! sound effect or redirection as necessary
            if (infoBoard.GetComponent<AudioSource>().isPlaying)
            {
                infoBoard.GetComponent<AudioSource>().Stop();
            }
            if (redirectionAudio != null) {
				infoBoard.GetComponent<AudioSource>().PlayOneShot(redirectionAudio);
                Invoke("Finished", redirectionAudio.length + delayBeforeAdvance);
            }
			else if(infoBoard.wrongSFX != null) {
				infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.wrongSFX);
                Invoke("Finished", infoBoard.wrongSFX.length + delayBeforeAdvance);
            }
            else
            {
                Invoke("Finished", delayBeforeAdvance);
            }

            // write appropriate entry to the logfile here
            outString += myEventNum.ToString() + "\t";
            outString += questionText + "\t";
            outString += "INCORRECT" + "\t";
            outString += "Answer given???" + "\t";
            outString += redirectionText;
            outString += "#";

            // If report card is used, write a line
            if (reportCard != null && reportCard.isActiveAndEnabled)
            {
                reportCard.writeLine(outString);
            }
        }           
	}   
}