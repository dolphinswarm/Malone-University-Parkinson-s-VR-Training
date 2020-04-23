using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);

/// <summary>
/// An event for advancing a script by toggling a specified button.
/// </summary>
public class ButtonEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Button Event Properties")]
    [Tooltip("True if this is an OVR button, false if this is a keyboard button")]
    public bool isOVRButton;                    // Is this an OVR button or a keyboard button?
    public OVRInput.RawButton targetButtonType;    // The OVR button type to check for.
    public KeyCode targetKey;                   // The keyboard type to check for.
    public float buttonPushDuration;            // How long should this button be pushed for?

    private float timePushedSoFar = 0.0f;
    private bool hasBeenActivated = false;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this pickup event.
    /// </summary>
    protected override void Initialize()
    {
        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// When the event should go...
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Button Event: Event #" + myEventNum);
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
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

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        base.Finished();
    }

    private void Update()
    {
        // If we want to check an OVR button or a key...
        if (isActive && !hasBeenActivated && (isOVRButton && OVRInput.GetDown(targetButtonType)) || (!isOVRButton && Input.GetKeyDown(targetKey)))
        {
            // Add to the time
            timePushedSoFar += Time.deltaTime;

            // If we have reached the threshold, activate click
            if (timePushedSoFar > buttonPushDuration)
            {
                hasBeenActivated = true;
                Clicked();
            }
        }
        // Else, reset the time
        else
        {
            timePushedSoFar = 0.0f;
        }
    }
}
