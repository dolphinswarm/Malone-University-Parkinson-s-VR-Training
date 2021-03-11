using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event, which uses the microphone to pick up if someone is speaking.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MicrophoneEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Microphone Event Handling")]
    public Animator animator;                // The animator for playing animations.
    public float baseline = 0.0f;           // The baseline for audio events.
    public bool loudFlag = false;           // A flag for when the user begins talking.
    public bool quietFlag = false;          // A flag for when the users end talking.

    private float[] clipSampleData = new float[1024];
    private AudioSource audioSource;         // The audio source.
    private bool validMic = false;           // Does this have a valid microphone?

    private bool hasBaselineBeenSet = false;
    private bool hasBeenTriggered = false;
    private bool checkerRunning = false;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // Get the audio source component attached to this object
        audioSource = GetComponent<AudioSource>();

        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Check if we have a valid mic
        validMic = (Microphone.devices.Length > 0);

        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Hide the pointers
        hideReticles = true;
        hideOnStart.Add(gameManager.currentReticle);
        showAtEnd.Add(gameManager.currentReticle);

        // If we do, then record it
        if (validMic)
            RecordAudio();

        // Else, wait 5 seconds then advance
        else
            StartCoroutine(WaitSeconds(5.0f));

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Microphone Event: Event #" + myEventNum + ")");

        // Add a beginning line to the report card manager
        //if (reportCard != null && reportCard.shouldWriteToReportCard)
        //    reportCard.writeLine(myEventNum + ".) Microphone Event (" + name + ")");


    }

    /// <summary>
    /// Starts recording the audio.
    /// </summary>
    private void RecordAudio()
    {
        // Get a recording from the primary microphone
        audioSource.clip = Microphone.Start(GetProperMicrophone(), true, 3, 44100);
        audioSource.loop = true;
        //audioSource.volume = 0.0f;

        // Play back the recording
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    /// <summary>
    /// Chooses the Oculus microphone for speech.
    /// </summary>
    /// <returns></returns>
    private string GetProperMicrophone()
    {
        // Check if a Rift mic is there
        // TODO update for Quest and other Oculus devices!
        foreach (string mic in Microphone.devices)
        {
            if (mic.Contains("Rift")) return mic;
        }

        return Microphone.devices[0];
    }

    /// <summary>
    /// If mic is active, get spectrum data.
    /// </summary>
    void Update()
    {
        // If we have a valid mic and the event is active...
        if (validMic && isActive)
        {
            // Get average volume for this second
            audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.Rectangular);
            float currentAverageVolume = clipSampleData.Average();

            // If baseline hasn't been set, set it
            if (!hasBaselineBeenSet)
            {
                baseline = currentAverageVolume;
                hasBaselineBeenSet = true;
            }

            // Print average
            //Debug.Log(currentAverageVolume);

            // Check the flags
            if (currentAverageVolume > baseline + 0.0001f)
                loudFlag = true;
            else if (loudFlag && currentAverageVolume < baseline + 0.0001f)
                quietFlag = true;
        }

        // If both flags triggered, advance
        if (loudFlag && quietFlag && !hasBeenTriggered && !checkerRunning)
        {
            StartCoroutine(CheckIfStillTalkingSeconds());
        }
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Stop the microphone and audiosource
        audioSource.Stop();
        Microphone.End(Microphone.devices[0]);

        // Check animation
        if (animator != null && animationName != "")
            SetAnimation(animator, animationName);

        // If we have an animator, play the animation
        if (animator != null)
            animator.SetTrigger("HasBeenTriggered");

        // Call base clicked
        base.Clicked();

        // play sound effect  &  move on to next thing
        //if (completedSFX != null)
        //{
        //    infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
        //    Invoke("Finished", completedSFX.length + delayBeforeAdvance);
        //}
        //else if (infoBoard.correctSFX != null)
        //{
        //    infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
        //    Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
        //}

        //else { Invoke("Finished", delayBeforeAdvance); }
    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        // Record the ending time
        //if (reportCard != null && reportCard.shouldWriteToReportCard)
        //    reportCard.writeLine(" - Elapsed Time: " + System.Math.Round(Time.time - startTime, 2) + " seconds");

        // Record to the report card
        //writeLine("eventName,elapsedTime,wasCorrect,providedAnswers,questionScore");
        if (reportCard != null && reportCard.shouldWriteToReportCard)
        {
            reportCard.writeLine(
                // eventName
                myEventNum + ".) " + name + "," +
                // elapsedTime
                System.Math.Round(Time.time - startTime, 2) + "," +
                // wasCorrect
                "n/a," +
                // providedAnswers
                "n/a," +
                // questionScore
                "n/a");
        }

        // Stop the microphone and audiosource, if playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Microphone.End(Microphone.devices[0]);
        }

        // Call the based finished
        base.Finished();
    }

    /// <summary>
    /// A backup coroutine, which waits 5 seconds then advances if no microphones are found.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Clicked();
    }

    /// <summary>
    /// A coroutine for checking when a user stops talking.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckIfStillTalkingSeconds()
    {
        // Turn on checker
        checkerRunning = true;

        // Wait two second
        yield return new WaitForSeconds(2.0f);

        // Get average volume for this second
        audioSource.GetSpectrumData(clipSampleData, 0, FFTWindow.Rectangular);
        float currentAverageVolume = clipSampleData.Average();

        // Turn on checker
        checkerRunning = false;

        // If not talking, stop
        if (currentAverageVolume < baseline + 0.0001f && audioSource.isPlaying && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            Clicked();
        } 
    }
}
