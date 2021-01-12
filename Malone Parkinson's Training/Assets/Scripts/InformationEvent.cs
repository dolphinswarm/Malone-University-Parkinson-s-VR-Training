using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple event for displaying information on the info board.
/// </summary>
public class InformationEvent : InfoBoardEvent
{
    // ======================================================== Variables
    [Header("Information Event UI and Properties")]
    public GameObject nextButton;               // The "next" button on the info board.
    [TextArea]
    public List<string> infoTextList;           // The text list to display.
    public int currentIndex = 0;                // The current index to display.

    // ======================================================== Methods
    /// <summary>
    /// Initialize this object.
    /// </summary>
    protected override void Initialize()
    {
        // Find next button, if not set
        if (nextButton == null)
            nextButton = GameObject.Find("Next");

        // Set it to inactive, if found
        if (nextButton != null)
            nextButton.SetActive(false);

        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// Start this event.
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
        // If we should skip due to lack of VR, then do so
        if (skipIfNoVRDetected && !OVRManager.isHmdPresent) nextEvent.Go();

        // If nothing is in the list...
        if (infoTextList.Count == 0)
        {
            // If we have info text, add it
            if (infoText != "")
            {
                infoTextList.Add(infoText);
            }
            // Else, print out default text
            else
            {
                infoTextList.Add("No text instructions have been specified! This is probably an error; you should not see this.");
            }
        }
        // Set the text to the first item in the info list
        infoBoard.ShowInstructions(infoTextList[0]);
        Debug.Log("Displaying message at index 0 to the console.");

        // Play voiceover, if not null
        if (voiceOver != null)
        {
            infoBoard.GetComponent<AudioSource>().PlayOneShot(voiceOver);
        }

        // Set next button
        if (nextButton == null)
            nextButton = infoBoard.next.gameObject;

        if (nextButton != null)
        {
            nextButton.SetActive(true);
            nextButton.GetComponent<ClickToNext>().informationEvent = this;
            nextButton.GetComponent<ClickToNext>().finishedClicking = false;
        }
        else Debug.LogError("Cannot find next button!");

        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Information Event: Event #" + myEventNum + ")");
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
    /// Finish this event, and begin transition to next event.
    /// </summary>
    public override void Finished()
    {
        // Deactivate next button
        if (nextButton != null)
            nextButton.SetActive(false);

        base.Finished();
    }
}


//// Add reticles to items to hide at start and show at end
//foreach (GameObject reticle in GameObject.FindGameObjectsWithTag("Reticle"))
//{
//    // Only show left hand
//    if (gameManager.dominantHand == DominantHand.LEFT && reticle.name == "Pointer-Left")
//    {

//    }

//    // Only show right hand
//    else if (gameManager.dominantHand == DominantHand.RIGHT && reticle.name == "Pointer-Right")
//    {
//        showOnStart.Add(reticle);
//        hideAtEnd.Add(reticle);
//    }

//    // Else, not needed, so hide
//    else
//    {
//        showOnStart.Add(reticle);
//        showAtEnd.Add(reticle);
//    }
//}
