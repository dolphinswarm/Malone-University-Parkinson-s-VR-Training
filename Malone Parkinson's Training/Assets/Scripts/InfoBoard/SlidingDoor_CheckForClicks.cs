using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor_CheckForClicks : MonoBehaviour {

    public slidingDoorToggle_2 doorManager;
    public bool requireTag = true;
    public string targetTag = "Reticle";
    public float consecutiveClickDelay = 0.5f;

    private float timeSinceLastClick = 0f;
    private bool clickedRecently = false;

    void Update() {
        // Track time elapsed since each click for a short while... prevents rapid-fire clicks
        // ...use 'clickedRecently' flag to determine if we need to track the elapsed time
        // NOTE:  Even though OnTriggerStay and Update both run every frame, Update is a better
        //  place for this code.  OnTriggerStay only runs while the user is in the trigger region.
        //  If they click and then leave the region, the timer wouldn't complete until they return
        //  and linger.  Impact is low with short delays, but it doesn't honor the requested delay time.
        if (clickedRecently) {
            // increment time counter each frame if user clicked recently
            timeSinceLastClick += Time.deltaTime;
            // check to see if enough time has elapsed to start allowing subsequent interaction
            if (timeSinceLastClick > consecutiveClickDelay) {
                // if it's been long enough, toggle 'clickedRecently' flag so we stop counting time
                clickedRecently = false;
                // ...and reset timer to zero
                timeSinceLastClick = 0f;
            }
        }    
    }


    void OnTriggerStay(Collider other) {
        // For performance reasons, nest the If statements instead of using compound logic testing...
        //  ...exit code-block quickly and with minimal method calls if possible

        // (1) Only proceed if it's been long enough since the last click
        if (!clickedRecently) {
            // (2) Only check for clicks if the the right object is colliding with the trigger
            if (!requireTag || (requireTag && other.CompareTag(targetTag)) ) {
                // (3) Check for input and Interact with door if valid input is received
                if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1")) {
                    // Tell doorManager to trigger door action
                    doorManager.InteractWithDoor();
                    //print ("interacted with door");
                    // note that we've 
                    clickedRecently = true;
                }
            }
        }
    }
}
