using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A destination event, where a user has to move to a certain location.
/// </summary>
public class DestinationEvent : InfoBoardEvent {
    // ======================================================== Variables
    [Header("Destination Properties")]
    public DestinationTrigger destinationTrigger;   // The destination trigger for this destination event
    public float lingerInTriggerForSec = 0.1f;      // The duration the player needs to stand in the destination event for it to activate. Passed to destination trigger
    public ParticleSystem particles;                // The child's particle system

    // ======================================================== Methods
    /// <summary>
    /// Initalizes this destination event and its child destination trigger.
    /// </summary>
    protected override void Initialize() {
        // Set info board and game manager
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager
        
        // Set the destination trigger
        if (destinationTrigger == null)
            destinationTrigger = GetComponentInChildren<DestinationTrigger>();

        // If particles is not set, set it
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // Initialize values of destinationTrigger
        destinationTrigger.infoBoard = infoBoard;
        destinationTrigger.owningEvent = this;
        destinationTrigger.lingerInTriggerForSec = lingerInTriggerForSec;
    }
    
    /// <summary>
    /// On event completion...
    /// </summary>
    public override void Clicked() {
        // Log stuff to file here
        base.Clicked();
    }

    /// <summary>
    /// Tells this event to begin.
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum) {
        // Print message to console
        Debug.Log("Starting Destination Event: Event #" + prevEventNum);

        // Re-enable the destination trigger
        destinationTrigger.enabled = true;
        destinationTrigger.isCurrentlyInteractable = true;

        // Start particle effect
        if (particles != null) particles.Play();

        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Show info text, if not null
        if (infoText != null) {
            infoBoard.ShowInstructions(infoText);
        }

        // Play voiceover, if not null
        if (voiceOver != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);
        }

        // Go to base event
        base.Go(prevEventNum);
    }

    /// <summary>
    /// Tells this event to finish and become deactivated.
    /// </summary>
    public override void Finished() {
        // Turn off particles
        particles.Stop();

        // Re-disable the destination trigger
        destinationTrigger.enabled = false;

        // Call the base finished class
        base.Finished();
    }

}
