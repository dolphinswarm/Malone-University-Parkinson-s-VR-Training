using UnityEngine;
using UnityEngine.UI;
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
public class QuestionEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Question Event Properties")]
    [TextArea]
    public string questionText;			                    // Text of question
    [TextArea]
    public string correctText;                              // Response text - if player answered correctly
    [TextArea]
    public string wrongText;                                // Response text - if player answered incorrectly
    public bool showAllOfTheAbove = false;                  // Display "All of the Above" as an option and use for toggling


    [Header("Audio")]
    public AudioClip wrongNarration;                        // Voiceover to play if answered incorrectly
    public AudioClip correctNarration;                      // Voiceover to play if answered incorrectly
    public AudioClip wrongSFX;                              // Voiceover to play if answered incorrectly
    public AudioClip correctSFX;                            // Voiceover to play if answered incorrectly


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
        Debug.Log("*** Starting + " + name + " (Question Event: Event #" + myEventNum + ")");

        // Add a beginning line to the report card manager
        //if (reportCard != null && reportCard.shouldWriteToReportCard)
        //    reportCard.writeLine(myEventNum + ".) Question Event (" + name + ")");
    }

    /// <summary>
    /// Marks an answer as sumbmitted, and prosses a list of open-ended answers.
    /// </summary>
    /// <param name="thingsSelected">A list of selected answers</param>
    public void ReportAnswer(List<AnswerManager> thingsSelected)
    {
        // we've been notified that an answer was given, as a selected list
        // These will be special cases... let's pass them on.
        GetComponent<OpenEndedQuestion>().ProcessAnswers(thingsSelected);

        // Submit this answer as "correct" and move on (can't get open-ended questions wrong)
        SubmitAnswer(true, thingsSelected, new List<AnswerManager>());
    }

    /// <summary>
    /// Handles whether a question has been answered correctly / incorrectly.
    /// </summary>
    /// <param name="answerIsCorrect"></param>
    public void SubmitAnswer(bool answerIsCorrect, List<AnswerManager> thingsSelected, List<AnswerManager> correctAnswers, float percentRight = 1.0f)
    {
		// save right/wrong state
		completedCorrectly = answerIsCorrect;

        // Set this question to answered
        hasBeenSubmitted = true;

        // If we answered correctly...
        if (answerIsCorrect) {

            // Set the correct text
            if (correctText != null && correctText != "")
                infoBoard.questionDisplay.GetComponent<Text>().text = correctText;

            // Check if the correct SFX is there
            if (correctSFX == null)
                correctSFX = infoBoard.correctSFX;

            // Play the sfx and narration
            StartCoroutine(PlayProperSoundAndNarration(correctSFX, correctNarration));
        }
        else {
            // Srt the incorrect text
            if (wrongText != null && wrongText != "")
                infoBoard.questionDisplay.GetComponent<Text>().text = wrongText;

            // Check if the correct SFX is there
            if (wrongSFX == null)
                wrongSFX = infoBoard.wrongSFX;

            // Play the sfx and narration
            StartCoroutine(PlayProperSoundAndNarration(wrongSFX, wrongNarration));
        }

        // Add the scores
        float score = percentRight;
        if (!isTutorialEvent)
        {
            reportCard.currentScore += score;
            reportCard.totalScore += 1;
        }

        // Make a string of provided answers
        string answerString = "";
        foreach (AnswerManager answer in thingsSelected)
        {
            answerString += answer.answerText + ";";
        }
        answerString.TrimEnd(';');
        if (answerString == "") answerString = "none";

        // Record to the report card
        //writeLine("eventName,elapsedTime,wasCorrect,providedAnswers,questionScore");
        if (reportCard != null && reportCard.shouldWriteToReportCard)
        {
            reportCard.writeLine(
                // eventName
                myEventNum + ".) " + name + "(" + questionText.Replace(',', '/') + ")" + "," +
                // elapsedTime
                Math.Round(Time.time - startTime, 2) + "," +
                // wasCorrect
                answerIsCorrect + "," +
                // providedAnswers
                answerString + "," +
                // questionScore
                Math.Round(score, 2));
        }
    }

    /// <summary>
    /// A coroutine for playing the proper sound effects, then ending the event
    /// </summary>
    /// <param name="sfx">The right/wrong sound effect.</param>
    /// <param name="narration">The proper narration.</param>
    /// <returns></returns>
    IEnumerator PlayProperSoundAndNarration(AudioClip sfx, AudioClip narration)
    {
        // Record the ending time
        //if (reportCard != null && reportCard.shouldWriteToReportCard)
        //    reportCard.writeLine(" - Elapsed Time: " + System.Math.Round(Time.time - startTime, 2) + " seconds");

        // Play the proper audio clip
        infoBoard.GetComponent<AudioSource>().PlayOneShot(sfx);

        // Wait
        yield return new WaitForSeconds(sfx.length + 0.05f);

        // If we have narration...
        if (narration != null)
        {
            // Play the proper audio clip
            infoBoard.GetComponent<AudioSource>().PlayOneShot(narration);

            // Wait
            yield return new WaitForSeconds(narration.length + 0.05f);
        } 
        // Else, just wait 3 seconds
        else yield return new WaitForSeconds(3.0f);

        // Call finished
        base.Finished();
    }
}