using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for advancing an info event.
/// </summary>
public class ClickToNext : InteractiveText {
    // ======================================================== Properties
    [Header("Current Information Event")]
    public InformationEvent informationEvent;
    public ShowScoreEvent showScoreEvent;
    public bool finishedClicking = false;

    // ======================================================== Methods
    // Inherits base start / initialize

    /// <summary>
    /// Once this object is selected, advances the event.
    /// </summary>
    protected override void Select()
    {
        // If we have an information event...
        if (informationEvent != null)
        {
            // To prevent spam-clicking...
            if (!finishedClicking)
            {
                // Get current number in list
                int currentItemInList = informationEvent.currentIndex + 1;

                // Are we on the last item?
                if (currentItemInList == informationEvent.infoTextList.Count)
                {
                    informationEvent.Clicked();
                    finishedClicking = true;
                }

                // Else, show the next instruction
                else
                {
                    informationEvent.infoBoard.ShowInstructions(informationEvent.infoTextList[currentItemInList]);
                    informationEvent.currentIndex++;
                    Debug.Log("Displaying message at index " + informationEvent.currentIndex + ".");
                }
            }
        }

        // Else, we have a score show event
        else if (showScoreEvent != null)
        {
            // To prevent spam-clicking...
            if (!finishedClicking)
            {
                showScoreEvent.Clicked();
            }
        }

    }
}