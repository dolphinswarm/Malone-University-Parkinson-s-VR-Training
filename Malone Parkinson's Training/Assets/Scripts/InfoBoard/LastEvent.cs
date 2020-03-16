using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEvent : InfoBoardEvent {

    //public LevelManager myLevelManager;
    public AudioClip endingAudio;
    public bool loadReportCardWhenFinished = true;
    public bool fadeScene;

     void Start()
    {
        //myLevelManager = GameObject.Find("Game Manager").GetComponent<LevelManager>();
    }

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
        GameObject.Find("Game Manager").GetComponent<HideCursor>().Show();
    }
}
