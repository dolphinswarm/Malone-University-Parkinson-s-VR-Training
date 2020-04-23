using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The trigger region for a destination event.
/// </summary>
public class DestinationTrigger : InteractiveObject {
    // ======================================================== Variables
    [Header("Destination Trigger Properties")]
    public float lingerInTriggerForSec = 0.1f;  // Time needed to cause the trigger to activate
    private float lingeredSoFar = 0f;           // The time the user has been in the trigger zone so far

    // ======================================================== Methods
    /// <summary>
    /// Initialize this destination trigger.
    /// </summary>
    protected override void Initialize() {
        // Call the base initialize
        base.Initialize();

        // Set the required to be the player by default
        requiredTag = "Player";

        // Turn on match tag by default
        matchTag = true;
    }

    /// <summary>
    /// On trigger enter, print a debug message.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter(Collider other)
    {
        // Print message to console
        Debug.Log(gameObject.name + " region has entered!");
    }

    /// <summary>
    /// When the trigger is entered, count down until the linger time is reached.
    /// </summary>
    /// <param name="other">The other collider.</param>
    protected override void OnTriggerStay(Collider other) {
        // If match tag is off, OR if we have the right tag...
        if ((!matchTag || (matchTag && other.gameObject.tag == requiredTag)) && isCurrentlyInteractable) {
            // Increment the linger time by frame time
            lingeredSoFar += Time.deltaTime;
            
            // Check for if lingered long enough. If so, let the game know.
            if (lingeredSoFar >= lingerInTriggerForSec) {
                // submit the answer
                Select();

                // Print message to console
                Debug.Log(gameObject.name + " region has been triggered!");

                // Set the lingered so far to be negative number so it won't trigger multiple times
                // Could use a boolean instead but ¯\_(ツ)_/¯
                lingeredSoFar = float.MinValue;
            }
        }
    }

    /// <summary>
    /// When leaving the trigger, reset the timer.
    /// </summary>
    /// <param name="other">The other collider.</param>
    protected override void OnTriggerExit(Collider other) {
        // Print message to console
        Debug.Log(gameObject.name + " region has been exited!");

        // Reset the linger timer.
        lingeredSoFar = 0f;
    }

}
