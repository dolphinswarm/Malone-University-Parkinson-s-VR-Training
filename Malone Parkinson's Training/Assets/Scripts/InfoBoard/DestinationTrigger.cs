using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationTrigger : InteractiveObjectOld {
    // ======================================================== Variables
    // watch for player to linger in the trigger region
    // matchTag should = "Player" -- set in Initialize
    public float lingerInTriggerForSec = 0.1f;  // Time needed to cause the trigger to activate
    private float lingeredSoFar = 0f;           // The time the user has been in the trigger zone so far
    public bool wasTriggered;                   // Has this been triggered?

    // ======================================================== Methods
    // Initialize
    protected override void Initialize() {
        requiredTag = "Player";
        matchTag = true;

        base.Initialize();
    }

    // OnTriggerStay: When the trigger is entered, count down until the linger time is reached
    protected override void OnTriggerStay(Collider other) {
        if (matchTag && other.gameObject.tag == requiredTag && !wasTriggered) {
            // increment linger time
            lingeredSoFar += Time.deltaTime;
            
            // check for lingering long enough
            if (lingeredSoFar >= lingerInTriggerForSec) {
                // submit the answer
                Select();
                Debug.Log(gameObject.name + " has been entered");
                wasTriggered = true;
            }
        }
    }

    // OnTriggerExit: When leaving the trigger, reset the timer
    protected override void OnTriggerExit(Collider other) {
        // reset linger time 
        if (matchTag && other.gameObject.tag == requiredTag) {
            lingeredSoFar = 0f;    
        }
    }

}
