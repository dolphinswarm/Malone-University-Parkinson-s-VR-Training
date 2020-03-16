using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Delivery Event Properties")]
    public PickupEvent pickupEvent;
    public bool hideImmmediatelyAfterDelivery = false;
    public bool hideAfterEvent = false;
    public ParticleSystem particles;
    public Transform deliveryLocaiton;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this delivery event.
    /// </summary>
    protected override void Initialize() {
        // Set the pickup event, if not set
        if (pickupEvent == null)
            pickupEvent = transform.parent.GetComponentInChildren<PickupEvent>();

        // If particles is not set, set it
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // Stop the particle system
        if (particles != null) particles.Stop();

        // Get delivery location, if not null
        if (deliveryLocaiton == null)
            deliveryLocaiton = GameObject.Find("Delivery Location").GetComponent<Transform>();

        // Call base initialize
        base.Initialize();
    }


    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Start particle effect
        if (particles != null) particles.Play();

        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Play voiceover, if not null
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
        Debug.Log("*** Starting Delivery Event***: Event #" + myEventNum);
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked() {
        // Hide, if should be hidden immediately
        if (hideImmmediatelyAfterDelivery)
        {
            pickupEvent.pickupObject.SetActive(false);
        }
        // Else, set the pickup object down
        else
        {
            pickupEvent.pickupObject.transform.position = deliveryLocaiton.transform.position;
            pickupEvent.pickupObject.transform.rotation = deliveryLocaiton.transform.rotation;
            pickupEvent.pickupObject.transform.parent = deliveryLocaiton.transform;
           
        }

        // Turn off particles
        particles.Stop();

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
    /// When event is finished...
    /// </summary>
    public override void Finished() {
        // If should be hidden, hide now
        if (hideAfterEvent)
        {
            pickupEvent.pickupObject.SetActive(false);
        }

        // Call base finished
        base.Finished();
    }

}
