using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Signifies that this event is the last in the event chain.
/// </summary>
public class LastEvent : InfoBoardEvent {
    // ======================================================== Variables
    //public AudioClip endingAudio;                   // Audio that should play when this event ends
    //public bool loadReportCardWhenFinished = true;  // Should we load the report card when finished?

    // ======================================================== Methods
    /// <summary>
    /// Starts this event.
    /// </summary>
    /// <param name="prevEventNum">The previous event number</param>
    public override void Go(int prevEventNum)
    {
        //// Use the base go
        //base.Go(prevEventNum);

        //// delay in ending will depend on presence of audio file
        //float delay = 0f;
        //if (endingAudio != null) {
        //    infoBoard.GetComponent<AudioSource>().PlayOneShot(endingAudio);
        //    delay = endingAudio.length;
        //}
        //Invoke("Finished", delay);

        // Load the title screen scene
        SceneManager.LoadScene("Title Scene");
    }

    /// <summary>
    /// Finish this event, then return to the title screen.
    /// </summary>
    public override void Finished()
    {
        //base.Finished();
    }
}
