using UnityEngine;
using System.Collections;

/* Todo list:
	- log data to file

// */

public class InfoObject : InfoBoardEvent {
	
	// info
	//public string infoText = "";			// text to display
	//public AudioClip voiceOver;             // none by default

	// duration
	public float showDurationSec = 5f;      // show for a few seconds by default
	public bool useClipLength = true;       // if true, show for duration of the voiceover clip, ignore previous
	public bool useExtraDelay = true;       // if true, show for a little longer after clip
	public float extraDelaySec = 1.25f;     //  ...this much longer, specifically

	// can click to skip ahead?   THESE ARE REPLICATED IN SkipHandler SCRIPT, WHICH DOES NOT INHERIT THIS
	public bool clickToSkip = true;         // can the user click to skip this?
	public float dontSkipFirstSec = 0.5f;   // impossible to skip first X sec (prevents rapid-fire click/skip of multiple items)


	// -----------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start () {
		// if a clip exists and we want to use it's length as the delay....
		if(useClipLength && voiceOver != null) {
			// ...then read it's delay and overwrite the standard delay
			showDurationSec = voiceOver.length;
			// ...and add any extra delay requested
			if(useExtraDelay) { showDurationSec += extraDelaySec; }
		}
		// Note that the InfoBoard object has a pointer to the logfile.
	}
	

	// Update is called once per frame
	void Update () {
		if(isActive) {									// only run this code if the info is active
			if (Time.time - startTime >= showDurationSec) {	// check to see how long we've been active
				Finished();									// ...and trigger end-state if we're finished
			}
		}
	
	}

    public override void Go(int prevEventNum) {
        // start info event
        infoBoard.SetInfo(gameObject.GetComponent<InfoObject>());  // pass a reference to this script
        //
        base.Go(prevEventNum);
    }


    // Finished does cleanup after the event
    //resets the board, hides things as needed, and goes to next thing (if any), logs data
    public override void Finished() {
        // write data to the log file
        //infoBoard.reportCardManager.writeLine("something to log");

        base.Finished();
    }

}
