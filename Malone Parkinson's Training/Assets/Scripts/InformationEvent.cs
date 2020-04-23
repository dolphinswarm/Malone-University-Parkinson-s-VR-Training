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
    public GameObject nextButton;
    public List<string> infoTextList;
    public int currentIndex = 0;

    // ======================================================== Methods
    /// <summary>
    /// Initialize this object.
    /// </summary>
    protected override void Initialize()
    {
        // Call base intialize
        base.Initialize();
    }

    /// <summary>
    /// Start this event.
    /// </summary>
    /// <param name="prevEventNum"></param>
    public override void Go(int prevEventNum)
    {
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
        
        // Activate next button. NOTE - if no next button, results in a softlock. FIX!! *****************************************************************************************************
        if (nextButton != null)
        {
            nextButton.SetActive(true);
            nextButton.GetComponent<ClickToNext>().clickHandler = this;
        }

        // Show / hide the appropriate reticles
        showOnStart.Add(gameManager.currentReticle);
        hideAtEnd.Add(gameManager.currentReticle);

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting Information Event***: Event #" + myEventNum);

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
