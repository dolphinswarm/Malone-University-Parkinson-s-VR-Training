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
    public bool isOVRButton;                        // Is this an OVR button or a keyboard button?
    public OVRInput.RawButton targetButtonType;     // The OVR button type to check for.
    public KeyCode targetKey;                       // The keyboard type to check for.
    public float buttonPushDuration;                // How long should this button be pushed for?
    public bool useDefaultInteract;                 // Should we use the default interact type?
    public OVRInput.RawButton defaultButton;        // The default hand button
    public bool showReticles = false;

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
        // If we should skip due to lack of VR, then do so
        if (skipIfNoVRDetected && !OVRManager.isHmdPresent) nextEvent.Go();

        // Set if we should make this a default OVR event
        if (gameManager.currentFPC.name == "Mouse and Keyboard Player Controller")
            isOVRButton = false;
        else
            isOVRButton = true;

        // Set the default button
        if (gameManager.dominantHand == DominantHand.RIGHT)
            defaultButton = OVRInput.RawButton.RIndexTrigger;
        else
            defaultButton = OVRInput.RawButton.LIndexTrigger;

        // If we have info text, add it
        if (infoText != "")
        {
            infoBoard.ShowInstructions(infoText);
        }

        // If we should show reticles, do so
        if (showReticles)
        {
            // Show / hide the appropriate reticles
            showOnStart.Add(gameManager.currentReticle);
            hideAtEnd.Add(gameManager.currentReticle);
        }

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Button Event: Event #" + myEventNum + ")");

        // Add a beginning line to the report card manager
        if (reportCard != null && reportCard.shouldWriteToReportCard)
            reportCard.writeLine(myEventNum + ".) Button Event (" + name + ")");
    }

    /// <summary>
    /// When this item is clicked...
    /// </summary>
    public override void Clicked()
    {
        // Call the base clicked
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

        base.Finished();
    }

    /// <summary>
    /// On frame update, check the button's press.
    /// </summary>
     void Update()
    {
        // If we want to check an OVR button or a key...
        if (isActive && !hasBeenActivated &&
            // OVR button without default interact
            ((isOVRButton && !useDefaultInteract && OVRInput.GetDown(targetButtonType)) || 
            // Keyboard button without default interact
            (!isOVRButton && !useDefaultInteract && Input.GetKeyDown(targetKey)) ||
            // OVR button with default interact
            (isOVRButton && useDefaultInteract && OVRInput.GetDown(defaultButton)) ||
            // Keyboard button with default interact
            (!isOVRButton && useDefaultInteract && Input.GetMouseButtonDown(0))))
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
