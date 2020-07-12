using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutEvent : InfoBoardEvent
{
    // ======================================================== Variables

    // ======================================================== Methods
    public override void Go(int prevEventNum)
    {
        // Display info text, if present
        if (infoText != "") infoBoard.ShowInstructions(infoText);

        // Calls the fadeout coroutine
        StartCoroutine("FadeOut");

        // Go to base event
        base.Go(prevEventNum);

        // Print message to console
        Debug.Log("*** Starting + " + name + " (Fadeout Event: Event #" + myEventNum + ")");
    }

    /// <summary>
    /// The coroutine for fading out the screen.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        // Start the fade out
        gameManager.screenFader.FadeOut();

        // Wait for 3 seconds
        yield return new WaitForSeconds(3.0f);

        // Toggle the finished event
        Finished();
    }
}
