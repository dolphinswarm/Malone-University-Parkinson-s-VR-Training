using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The delivery state of this object.
/// </summary>
public enum DeliveryState { HIDE_IMMEDIATELY, HIDE_AFTER_EVENT, NO_HIDE }

/// <summary>
/// An event for delivering an object to a location.
/// </summary>
public class DeliveryEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Delivery Event Properties")]
    public PickupEvent pickupEvent;
    public DeliveryState hideType = DeliveryState.NO_HIDE;
    public ParticleSystem particles;
    public List<GameObject> stuffToShowOnDelivery;
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

        // Start particle effect
        if (particles != null) particles.Play();

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Delivery Event: Event #" + myEventNum + ")");
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Show everything that should be hidden
        foreach (GameObject showThing in stuffToShowOnDelivery)
        {
            showThing.SetActive(true);
        }

        // Hide, if should be hidden immediately
        if (hideType == DeliveryState.HIDE_IMMEDIATELY)
        {
            pickupEvent.pickupObject.SetActive(false);
        }
        // Else, set the pickup object down
        else if (hideType == DeliveryState.NO_HIDE)
        {
            pickupEvent.pickupObject.transform.position = deliveryLocaiton.transform.position;
            pickupEvent.pickupObject.transform.rotation = deliveryLocaiton.transform.rotation;
            pickupEvent.pickupObject.transform.parent = deliveryLocaiton.transform;
            pickupEvent.pickupObject.GetComponent<InteractiveObject>().Dim();
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
        // Record to the report card
        //writeLine("eventName,elapsedTime,wasCorrect,providedAnswers,questionScore");
        if (reportCard != null && reportCard.shouldWriteToReportCard)
        {
            reportCard.writeLine(
                // eventName
                myEventNum + ".) " + name + "," +
                // elapsedTime
                System.Math.Round(Time.time - startTime, 2));
        }

        // If should be hidden, hide now
        if (hideType == DeliveryState.HIDE_AFTER_EVENT)
        {
            pickupEvent.pickupObject.SetActive(false);
        }

        // Turn off particles
        if (particles != null)
            particles.Stop();

        // Call base finished
        base.Finished();
    }

}
