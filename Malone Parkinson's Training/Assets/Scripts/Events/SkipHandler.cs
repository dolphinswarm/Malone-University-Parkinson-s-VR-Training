using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipHandler : MonoBehaviour {

    // can click to skip ahead?
    public GameObject sourceEvent;
    public bool clickToSkip = false;        // can the user click to skip this?
    public float dontSkipFirstSec = 0.5f;   // impossible to skip first X sec (prevents rapid-fire click/skip of multiple items)
    bool enableSkip = false;             // internal flag to allow / deny skipping
    public float startTime;

    public GameObject skipButton;

    // Use this for initialization
    public void Skip () {
        // reset skip settings for next time
        clickToSkip = false;
        enableSkip = false;
        dontSkipFirstSec = 0.5f;

        // hide skip button until needed again
        skipButton.SetActive(false);

        // then tell source event to go
        sourceEvent.SendMessage("Finished", SendMessageOptions.DontRequireReceiver);
	}


    public void Go() {
        // reset timer whenever this object is activated
        startTime = Time.time;
        //Debug.Log(gameObject.name + " resetting skip timer");
    }
	
	// Update is called once per frame
	void Update () {
        // NOTE:  we're not trying to pick up clicks here... just enabling / disabling the skip ability
        // i.e., if skipping is desired, and we've exceed the initial no-skip period, enable skipping ....but only toggle flag once
        if (!enableSkip                                                                 // if we haven't enabled skipped yet
            && (Time.time - startTime >= dontSkipFirstSec)                              // && the time elapsed is above threshold 
            && clickToSkip) {                                                           // && the item is skipable
            // then....
            enableSkip = true;                                                      // prevent this code from running again
            skipButton.SetActive(true);                                             // turn on the skip button
            skipButton.GetComponent<ClickToSkip>().informationEvent = gameObject;       // grab the handle to our click handler
            //Debug.Log(gameObject.name + " enabled skipping");
        }

        // if we shouldn't be able to skip on this item, disable the button
        if(clickToSkip == false) {
            skipButton.SetActive(false);
        }
        else {      // ...otherwise, we should enable it???  or shouldn't we let the code block above do that.
            // skipButton.SetActive(true);
        }
    }
}
