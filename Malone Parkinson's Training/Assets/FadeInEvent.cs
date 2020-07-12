using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEvent : InfoBoardEvent
{
    // ======================================================== Variables

    // ======================================================== Methods
    public override void Go(int prevEventNum)
    {
        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Calls the fadeIn corInine
        StartCoroutine("FadeIn");

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Fadein Event: Event #" + myEventNum + ")");
    }

    /// <summary>
    /// The corInine for fading In the screen.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        // Start the fade In
        gameManager.screenFader.FadeIn();

        // Wait for 3 seconds
        yield return new WaitForSeconds(3);

        // Toggle the finished event
        Finished();
    }
}
