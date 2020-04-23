using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Signifies that this event is the last in the event chain.
/// </summary>
public class LastEvent : InfoBoardEvent {
    // ======================================================== Variables
    public AudioClip endingAudio;                   // Audio that should play when this event ends
    public bool loadReportCardWhenFinished = true;  // Should we load the report card when finished?
    public bool fadeScene;                          // Should we fade the scene?

    // ======================================================== Methods
    public override void Go(int prevEventNum) {
        base.Go(prevEventNum);

        // delay in ending will depend on presence of audio file
        float delay = 0f;
        if (endingAudio != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(endingAudio);
            delay = endingAudio.length;
        }
        Invoke("Finished", delay);
    }


    public override void Finished() {
        // two possible ending routes...
        if (loadReportCardWhenFinished) {
           //myLevelManager.LoadReportCard();
        }
        else {
            //myLevelManager.AllDone();
        }

        //reveal the cursor for the credits  
        //GameObject.Find("Game Manager").GetComponent<HideCursor>().Show();
    }
}
