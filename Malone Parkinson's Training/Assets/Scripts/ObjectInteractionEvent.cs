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
    public List<GameObject> interactionObjects;         // The list of possible interaction objects.
    public bool hideBeforeInteraction = false;          // Should the interactive object be hidden before its event?
    public bool interactWithAllObjects = false;         // Do we need to interact with all objects? NEED TO IMPLEMENT!!!!!!!!!!!!!!!!!!!!!!!
    public ParticleSystem particles;                    // The particle system used for display. ALWAYS IN CHILD

    [System.Obsolete("Don't use this - instead, an animation component plays in the child")]
    public SpecialAnimationInstructions specialAnimationInstructions; // <------- Removed in favor of animation component

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // If particles is not set, set it
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // Stop the particles
        if (particles != null)
            particles.Stop();

        // Look for any special animation instructions
        if (particleSystemForAnimation == null)
            particleSystemForAnimation = GetComponent<ParticleSystem>();

        // If the particle system is equal to the particles, remove the particles
        if (particleSystemForAnimation != null && particleSystemForAnimation == particles) particles = null;

        // Do some handling on each interactive object
        foreach (GameObject interactiveObject in interactionObjects)
        {
            // Add particle system to object, if not null
            if (particleSystemForAnimation != null) { 
                ParticleContainer particleContainer = interactiveObject.GetComponentInParent<ParticleContainer>();
                if (particleContainer != null) particleContainer.particleSystemForAnimation = particleSystemForAnimation;
            }

            // If we should hide before interaction, hide this object
            if (hideBeforeInteraction)
                interactiveObject.GetComponent<InteractiveObject>().SetHideBeforeEvent(true);

            // Make this object the owning game event
            interactiveObject.GetComponent<InteractiveObject>().owningEvent = this;
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
            // Show the object, if hidden
            if (hideBeforeInteraction) interactiveObject.SetActive(true);

            // Enable the interactive object
            InteractiveObject intObj = interactiveObject.GetComponent<InteractiveObject>();
            intObj.enabled = true;
            intObj.isCurrentlyInteractable = true;

            // Change the interaction type, if reliant on oculus hands
            if (intObj.highlightType == HighlightType.HAND_DISTANCE && gameManager.controlType == ControlType.MOUSE_KEYBOARD)
            {
                intObj.highlightType = HighlightType.POINTAT;
            }
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
        // Check if we should continue or not. if we should...
        if ((interactWithAllObjects && CheckChildrenForActive()) || !interactWithAllObjects)
        {
            // Turn off particles
            if (particles != null) particles.Stop();

            // Set all reliant's interactive object scripts to inactive
            foreach (GameObject interactiveObject in interactionObjects)
            {
                // Dim the ineraction objects
                interactiveObject.GetComponent<InteractiveObject>().Dim();

                // Enable the interactive object
                InteractiveObject intObj = interactiveObject.GetComponent<InteractiveObject>();
                intObj.isCurrentlyInteractable = false;
                intObj.enabled = false;
                intObj.isMouseOver = false;
            }

            // If we have a completed sound effect, play it
            if (completedSFX != null)
            {
                infoBoard.GetComponent<AudioSource>().PlayOneShot(completedSFX);
                Invoke("Finished", completedSFX.length + delayBeforeAdvance);
            }

            // If not, check the info board for one then play it
            else if (infoBoard.correctSFX != null)
            {
                infoBoard.GetComponent<AudioSource>().PlayOneShot(infoBoard.correctSFX);
                Invoke("Finished", infoBoard.correctSFX.length + delayBeforeAdvance);
            }
            // Else, invoke finished normally
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

    /// <summary>
    /// Check this event's interactive objects, if we need to interact with all.
    /// </summary>
    /// <returns></returns>
    private bool CheckChildrenForActive()
    {
        // Create a temp bool
        bool haveAllbeenSelected = true;

        // Loop through each interactive object, checking to see if any are still interactable
        foreach (GameObject interactiveObject in interactionObjects)
        {
            // If we find one, break
            if (interactiveObject.GetComponent<InteractiveObject>().isCurrentlyInteractable)
            {
                haveAllbeenSelected = false;
                break;
            }
        }

        // Return the value
        return haveAllbeenSelected;
    }
}