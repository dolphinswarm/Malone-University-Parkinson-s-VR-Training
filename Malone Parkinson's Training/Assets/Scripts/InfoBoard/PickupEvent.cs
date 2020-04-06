using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event for picking up an object
/// </summary>
public class PickupEvent : InfoBoardEvent {
    // ======================================================== Variables
    [Header("Pickup Event Properties")]
    public bool hideBeforePickup = false;
    public bool hideAfterPickup = false;
    public GameObject pickupObject;
    public ParticleSystem particles;
    public SpecialTransformInstructions transformInstructions;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize() {
        // If the pickup object is not set, get it
        if (pickupObject == null)
            pickupObject = GetComponentInChildren<InteractiveObject>().GetGameObject();

        // If the object should be hidden before pickup, hide it
        if (hideBeforePickup) pickupObject.GetComponent<InteractiveObject>().SetHideBeforeEvent(true);

        // If particles is not set, set it
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // Stop the particle system
        if (particles != null) particles.Stop();

        // Call base intialize
        base.Initialize();
    }
   
    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum) {
        // Show the object, if hidden
        if (hideBeforePickup) pickupObject.SetActive(true);

        // Set the pickup object to interactive
        InteractiveObject intObj = pickupObject.GetComponent<InteractiveObject>();
        intObj.enabled = true;
        intObj.isCurrentlyInteractable = true;

        // Start particle effect
        if (particles != null) particles.Play();

        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Play voiceover, if present
        if (voiceOver != null) infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);

        // If using Oculus, hide the reticle
        if (gameManager.controlType == ControlType.OCULUS)
        {
            hideOnStart.Add(gameManager.currentReticle);
            showAtEnd.Add(gameManager.currentReticle);
        }
        else
        {
            showOnStart.Add(gameManager.currentReticle);
            hideAtEnd.Add(gameManager.currentReticle);
        }

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Pickup Event***: Event #" + myEventNum);
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Hide Item that is picked up if requested
        if (hideAfterPickup)
        {
            pickupObject.SetActive(false);
        }
        // Else, attach to hand
        else
        {
            // If oculus...
            if (gameManager.controlType == ControlType.OCULUS)
            {
                // If instructions specified, follow them
                if (transformInstructions != null)
                {
                    pickupObject.transform.SetParent(transformInstructions.parentObject.transform);
                    pickupObject.transform.localPosition = transformInstructions.positon;
                    pickupObject.transform.localRotation = Quaternion.Euler(transformInstructions.rotation);
                }

                // Else, attach to current hand by default
                else
                {
                    pickupObject.transform.SetParent(gameManager.currentHand.transform);
                }
            }

            // If mouse and keyboard...
            else if (gameManager.controlType == ControlType.MOUSE_KEYBOARD)
            {
                pickupObject.transform.position = gameManager.currentHand.transform.position;
                pickupObject.transform.SetParent(gameManager.currentHand.transform);
            }
        }

        // Turn off particles
        particles.Stop();

        // play sound effect  &  move on to next thing
        if (completedSFX != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
            Invoke("Finished", completedSFX.length + delayBeforeAdvance);
        }
        else if (infoBoard.correctSFX != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
            Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
        }
        else { Invoke("Finished", delayBeforeAdvance); }
    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished() {
        base.Finished();
    }
}
