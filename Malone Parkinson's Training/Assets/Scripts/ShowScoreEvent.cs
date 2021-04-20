using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A simple event for displaying information on the info board.
/// </summary>
public class ShowScoreEvent : InfoBoardEvent
{
    // ======================================================== Variables
    public GameObject nextButton;               // The "next" button on the info board.
    public GameObject scoreBoard;
    public Text scoreText;
    public Text timeText;

    // ======================================================== Methods
    /// <summary>
    /// Initialize this object.
    /// </summary>
    protected override void Initialize()
    {
        // Find next button, if not set
        if (nextButton == null)
            nextButton = GameObject.Find("Next");

        // Set it to inactive, if found
        if (nextButton != null)
            nextButton.SetActive(false);
        
        // Find next button, if not set
        if (scoreBoard == null)
            scoreBoard = GameObject.Find("Score Board");

        // If found, set its components
        if (scoreBoard != null)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            timeText = GameObject.Find("Time Text").GetComponent<Text>();
            scoreBoard.SetActive(false);
        }

        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// Start this event.
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Show the text
        infoBoard.ShowInstructions("");

        // Set the game manager's absolute end time
        gameManager.absoluteEndTime = Time.time;

        // Set the various items
        scoreText.text = Math.Round(reportCard.currentScore, 2) + " / " + Mathf.Round(reportCard.totalScore) + " (" + Math.Round((reportCard.currentScore / reportCard.totalScore) * 100, 2) + "%)";
        float totalTime = gameManager.absoluteEndTime - gameManager.absoluteStartTime;
        int minutes = (int)totalTime / 60;
        int seconds = (int)totalTime % 60;
        timeText.text = minutes + " minutes, " + seconds + " seconds";

        // Show the scorecard
        scoreBoard.SetActive(true);

        // Play voiceover, if not null
        if (voiceOver != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);
        }

        // Set next button
        if (nextButton == null)
            nextButton = infoBoard.next.gameObject;

        if (nextButton != null)
        {
            nextButton.SetActive(true);
            nextButton.GetComponent<ClickToNext>().informationEvent = null;
            nextButton.GetComponent<ClickToNext>().showScoreEvent = this;
            nextButton.GetComponent<ClickToNext>().finishedClicking = false;
        }
        else Debug.LogError("Cannot find next button!");

        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Score View Event: Event #" + myEventNum + ")");
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Show the scorecard
        scoreBoard.SetActive(false);

        // Call the base clicked
        base.Clicked();
    }

    /// <summary>
    /// Finish this event, and begin transition to next event.
    /// </summary>
    public override void Finished()
    {
        // Deactivate next button
        if (nextButton != null)
            nextButton.SetActive(false);

        // Hide the scorecard
        scoreBoard.SetActive(false);

        base.Finished();
    }
}
