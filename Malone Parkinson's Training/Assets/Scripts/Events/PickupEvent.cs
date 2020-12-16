using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An event for picking up an object
/// </summary>
public class PickupEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Pickup Event Properties")]
    public GameObject pickupObject;
    public ParticleSystem particles;
    public List<GameObject> stuffToHideOnPickup;
    public bool placeInMainHand = true;

    [Header("Transform Properties")]
    public bool useSpecialInstructions = false;
    public Vector3 pickupObjectPosition;
    public Vector3 pickupObjectRotation;
    public bool mirrorForOppositeHand = true;
    public Vector3 leftPickupObjectPosition;
    public Vector3 leftPickupObjectRotation;
    public Vector3 rightPickupObjectPosition;
    public Vector3 rightPickupObjectRotation;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize() {
        // If the pickup object is not set, get it
        if (pickupObject == null)
            pickupObject = GetComponentInChildren<InteractiveObject>().GetGameObject();

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
    public override void Go(int prevEventNum)
    {
        // Show the object
        pickupObject.SetActive(true);

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
        Debug.Log("*** Starting + " + name + " (Pickup Event: Event #" + myEventNum + ")");

        // Add a beginning line to the report card manager
        if (reportCard != null && reportCard.shouldWriteToReportCard)
            reportCard.writeLine(myEventNum + ".) Pickup Event (" + name + ")");
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Hide everything that should be hidden
        foreach (GameObject hideThing in stuffToHideOnPickup)
        {
            hideThing.SetActive(false);
        }

        // Attach to hand
        if (pickupObject.activeSelf == true)
        {
            // BREAKS WHEN THE USER CLICKS WITH OPPOSITE HAND---- FIX! **************************************
            // If we want to place in the main hand...
            if (placeInMainHand)
            {
                // Attach the item to the hand
                if (gameManager.controlType == ControlType.MOUSE_KEYBOARD)
                {
                    pickupObject.transform.position = gameManager.currentHand.transform.position;
                }
                pickupObject.transform.SetParent(gameManager.currentHand.transform);

                // If we should use special instructions...
                if (useSpecialInstructions)
                {
                    // If we should mirror...
                    if (mirrorForOppositeHand)
                    {
                        pickupObject.transform.localPosition = pickupObjectPosition;
                        pickupObject.transform.localRotation = Quaternion.Euler(pickupObjectRotation);
                    }
                    // Else, if we want to use actual instructions...
                    else
                    {
                        // If right-handed, place in right hand
                        if (gameManager.dominantHand == DominantHand.RIGHT)
                        {
                            pickupObject.transform.localPosition = rightPickupObjectPosition;
                            pickupObject.transform.localRotation = Quaternion.Euler(rightPickupObjectRotation);
                        }
                        // If left-handed, place in left hand
                        else
                        {
                            pickupObject.transform.localPosition = leftPickupObjectPosition;
                            pickupObject.transform.localRotation = Quaternion.Euler(leftPickupObjectRotation);
                        }
                    }
                }
            }
            // Else, place in the OPPOSITE hand
            else
            {
                // Attach the item to the hand
                if (gameManager.controlType == ControlType.MOUSE_KEYBOARD)
                {
                    pickupObject.transform.position = gameManager.offHand.transform.position;
                }
                pickupObject.transform.SetParent(gameManager.offHand.transform);

                // If we should use special transform instructions...
                if (useSpecialInstructions)
                {
                    // If we should mirror...
                    if (mirrorForOppositeHand)
                    {
                        pickupObjectPosition.x *= -1;
                        //pickupObjectRotation.y += 180;

                        pickupObject.transform.localPosition = pickupObjectPosition;
                        pickupObject.transform.localRotation = Quaternion.Euler(pickupObjectRotation);
                    }
                    // Else, if we want to use actual instructions...
                    else
                    {
                        // If right-handed, place in left
                        if (gameManager.dominantHand == DominantHand.RIGHT)
                        {
                            pickupObject.transform.localPosition = leftPickupObjectPosition;
                            pickupObject.transform.localRotation = Quaternion.Euler(leftPickupObjectRotation);
                        }
                        // If left-handed, place in right
                        else
                        {
                            pickupObject.transform.localPosition = rightPickupObjectPosition;
                            pickupObject.transform.localRotation = Quaternion.Euler(rightPickupObjectRotation);
                        }
                    }
                }
            }

            // Set the pickup object to its default material
            pickupObject.GetComponent<InteractiveObject>().Dim();
        }

        // Turn off particles
        if (particles != null)
            particles.Stop();

        // Call base clicked
        base.Clicked();
    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        // Record the ending time
        if (reportCard != null && reportCard.shouldWriteToReportCard)
            reportCard.writeLine(" - Elapsed Time: " + System.Math.Round(Time.time - startTime, 2) + " seconds");

        // If the pickup object isn't picked up, un-activate it
        InteractiveObject interactiveObject = pickupObject.GetComponent<InteractiveObject>();
        if (interactiveObject.isCurrentlyInteractable)
            interactiveObject.isCurrentlyInteractable = false;

        // Turn off particles
        if (particles != null)
            particles.Stop();

        // Call the base finished
        base.Finished();
    }
}
