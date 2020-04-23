using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for advancing an info event.
/// </summary>
public class ClickToNext : InteractiveText {
    // ======================================================== Properties
    [Header("Current Information Event")]
    public InformationEvent clickHandler;

    // ======================================================== Methods
    /// <summary>
    /// Initializes this script.
    /// </summary>
    void Start()
    {
        // If info board is not set, then set it
        if (infoBoard == null)
            infoBoard = FindObjectOfType<InfoBoardUI>(); // Should only be one InfoBoardUI (hopefully)

        // If game manager is not set, then set it
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>(); // Should only be one GameManager

        // Call the remainder of the base start
        base.Start();
    }

    /// <summary>
    /// Once this object is selected, advances the event.
    /// </summary>
    protected override void Select() {
        // Get current number in list
        int currentItemInList = clickHandler.currentIndex + 1;

        // Are we on the last item?
        if (currentItemInList == clickHandler.infoTextList.Count)
        {
            clickHandler.GetComponent<InformationEvent>().Finished();
        }

        // Else, show the next instruction
        else
        {
            clickHandler.infoBoard.ShowInstructions(clickHandler.infoTextList[currentItemInList]);
            clickHandler.currentIndex++;
            Debug.Log("Displaying message at index " + clickHandler.currentIndex + " to the console.");
        }
    }
}