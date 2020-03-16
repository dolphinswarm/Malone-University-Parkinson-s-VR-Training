using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseEvent : InfoBoardEvent {

    public slidingDoorToggle_2 doorObject;
    public bool continuousEvent = true; // always check for door closure.... or just on demand
    public AudioClip warningAudio;
    public AudioClip redirectAudio;

    public GameObject[] finalCheckRegions;   // triggers that check for door to be closed

    // Use this for initialization
    protected override void Initialize () {
        if (continuousEvent) { Go( ); }
        EnableChecks(false);  // turn off regions that check for door to be closed
    }

    // Use this internally to check the state of the door for warning / final check
    private bool DoorInProperState() {
        // Ideally, door should be closed or in the process of closing
        // If this is not the case, we need to react
        if (doorObject.state == DoorState.closed || doorObject.state == DoorState.closing) {
            return true;
        }
        else { return false; }
    }

    // Called externally... tells the event to check the door and issue applicable warnings
    public void CheckDoor() {
        // if door is not in proper state.... issue warning and log
        if (!DoorInProperState()) {
            // play the warning audio
            infoBoard.GetComponent<AudioSource>().PlayOneShot(warningAudio);
            // log the event in the log file
            infoBoard.Log("'Close Door' warning issued for " + doorObject.name + " at " + Time.time.ToString() );
            
            // enable final check regions only AFTER warning is issued
            EnableChecks(true);  
        }
        // if door is in proper state, do nothing
        else {
            // NOTE: if final checks are disabled here, logging won't show that the user 
            //      returned to close the door after a warning.
            //EnableChecks(false); 
        }
    }

    // Called externally... tells the event to check the door and log results
    public void FinalCheck() {
        // if door is in proper state, log this
        if (DoorInProperState()) {
            infoBoard.Log("Sliding door: " + doorObject.name + " was closed properly. Checked at " + Time.time.ToString());
        }
        // if door is not in proper state....
        else { 
            // play the redirect audio
            infoBoard.GetComponent<AudioSource>().PlayOneShot(redirectAudio);
            // log the event in the log file
            infoBoard.Log("Sliding door: " + doorObject.name + " was NOT closed. Checked at " + Time.time.ToString());
        }

        // disable final check regions after a final check has been performed,
        //  otherwise the check may be performed multiple times, e.g. as the user returns
        EnableChecks(false);

        // if this is not a continuous event, then we need to trigger the next event
        if (!continuousEvent) { Finished(); }
    }


    private void EnableChecks(bool onOff) {
        foreach (GameObject triggerRegion in finalCheckRegions) {
            triggerRegion.SetActive(onOff);
        }
    }
}
