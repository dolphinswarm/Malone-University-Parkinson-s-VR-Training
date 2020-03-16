using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Locator Event Properties")]
    public List<LocatorObject> targets;
    public AudioClip minorDing;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // Fill the target list
        foreach (LocatorObject locObject in GetComponentsInChildren<LocatorObject>())
        {
            targets.Add(locObject);
            locObject.gameObject.SetActive(false);
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
        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Play voiceover, if present
        if (voiceOver != null) infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);

        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Set the target list to active
        foreach (LocatorObject locObject in targets)
        {
            locObject.gameObject.SetActive(true);
        }

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Locator Event***: Event #" + myEventNum);

    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Check size of list. If empty...
        if (targets.Count <= 0)
        {
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
        // Else, play minor ding
        else
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(minorDing);
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
