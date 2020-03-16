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

    // ======================================================== Methods
    /// <summary>
    /// Initalizes this destination event and its child destination trigger.
    /// </summary>
    protected override void Initialize() {
        // Set info board and destination trigger
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>();
        if (destinationTrigger == null)
            destinationTrigger = GetComponentInChildren<DestinationTrigger>();

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameSettings>(); // Should only be one GameManager

        // Initialize values of destinationTrigger
        destinationTrigger.infoBoard = infoBoard;
        destinationTrigger.owningEvent = this;
        destinationTrigger.lingerInTriggerForSec = lingerInTriggerForSec;

        // ensure that visual indicators and click-functionality are disabled until needed
        destinationTrigger.transform.parent.gameObject.SetActive(false);
        destinationTrigger.visibleObjectParentNode.gameObject.SetActive(false);
    }
    
    // WHAT DOES THE CLICK DO???
    public override void Clicked() {
        // Log stuff to file here
        Debug.Log(name + ": Click!");

        // Hide Item that is picked up if requested
        destinationTrigger.visibleObjectParentNode.gameObject.SetActive(false);
        
        // play sound effect  &  move on to next thing
        if (completedSFX != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
            Invoke("Finished", completedSFX.length + delayBeforeAdvance);
        }
        else if (infoBoard.correctSFX != null) {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
            Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
        }

        else { Invoke("Finished", delayBeforeAdvance); }
    }

    /// <summary>
    /// Tells this event to begin.
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum) {
        // Print message to console
        Debug.Log("Starting Destination Event: Event #" + prevEventNum);

        // Enable the visual indicators and click-functionality
        destinationTrigger.transform.parent.gameObject.SetActive(true);
        destinationTrigger.visibleObjectParentNode.gameObject.SetActive(true);

        // Add reticles to items to hide at start and show at end
        foreach (GameObject reticle in GameObject.FindGameObjectsWithTag("Reticle"))
        {
            hideOnStart.Add(reticle);
            showAtEnd.Add(reticle);
        }

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
        destinationTrigger.transform.parent.gameObject.SetActive(false);
        base.Finished();
    }

}
