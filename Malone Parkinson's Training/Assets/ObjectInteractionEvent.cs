using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class for an interaction object.
/// </summary>
public class ObjectInteractionEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Interaction Event Properties")]
    public List<GameObject> interactionObjects;
    public ParticleSystem particles;
    public SpecialAnimationInstructions specialAnimationInstructions;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // If the pickup object is not set, get it

        // If particles is not set, set it
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // Stop the particle system
        if (particles != null) particles.Stop();

        // Set all reliant's interactive object scripts to inactive
        foreach (GameObject interactiveObject in interactionObjects)
        {
            interactiveObject.GetComponent<InteractiveObject>().enabled = false;
        }

        // Call base intialize
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

        // Play voiceover, if present
        if (voiceOver != null) infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);

        // Set all reliant's interactive object scripts to active
        foreach (GameObject interactiveObject in interactionObjects)
        {
            interactiveObject.GetComponent<InteractiveObject>().enabled = true;
        }

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
        Debug.Log("*** Starting Interaction Event: Event #" + myEventNum);
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Turn off particles
        particles.Stop();

        // Dim the interaction objects
        foreach (GameObject interactiveObject in interactionObjects)
        {
            interactiveObject.GetComponent<InteractiveObject>().Dim();
        }

        // Set all reliant's interactive object scripts to inactive
        foreach (GameObject interactiveObject in interactionObjects)
        {
            interactiveObject.GetComponent<InteractiveObject>().enabled = false;
        }

        // If special animation instructions, play them
        if (specialAnimationInstructions != null)
        {
            // If we have a completed sound effect, play it
            if (completedSFX != null)
            {
                infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
            }
            // If not, check the info board for one then play it
            else if (infoBoard.correctSFX != null)
            {
                infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
            }
            // Else, just call finished normally.
            specialAnimationInstructions.PlayAnimation();
        }
        // Else, play sound effect  &  move on to next thing
        else
        {
            // If we have a completed sound effect, play it
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
            // Else, just call finished normally.
            else { Invoke("Finished", delayBeforeAdvance); }
        }

    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        base.Finished();
    }
}