using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorialEvent : InfoBoardEvent
{
    // ======================================================== Variables

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
        Debug.Log("*** Starting End Tutorial Event***: Event #" + myEventNum);

        // Set the info board message
        infoBoard.ShowInstructions(infoText);

        // Get the game manager fade
        gameManager.StartSimulation(this);

    }

    /// <summary>
    /// When event is finished...
    /// </summary>
    public override void Finished()
    {
        base.Finished();
    }
}
